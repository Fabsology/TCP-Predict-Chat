using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatServer
{
    static List<StreamWriter> clientWriters = new List<StreamWriter>();  // List to hold writers for connected clients
    static List<string> messageHistory = new List<string>();  // List to store chat message history
    static object clientLock = new object();  // Lock object to synchronize access to shared resources

    static void Main(string[] args)
    {
        Console.WriteLine("Enter the IP address for listening (IPv4):");

        TcpListener server = null;
        string ipAddressString = Console.ReadLine();

        try
        {
            IPAddress ipAddress = IPAddress.Parse(ipAddressString);
            int port = 8888;
            server = new TcpListener(ipAddress, port);
            server.Start();

            Console.WriteLine("Server started. Waiting for connections...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();  // Accept incoming client connection
                Console.WriteLine("New client connected.");

                NetworkStream stream = client.GetStream();  // Get the network stream for communication
                StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

                lock (clientLock)
                {
                    foreach (string message in messageHistory)
                    {
                        byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                        stream.Write(messageBytes, 0, messageBytes.Length);  // Send message history to the newly connected client
                    }

                    clientWriters.Add(writer);  // Add the writer to the list of client writers
                }

                Thread clientThread = new Thread(() => HandleClient(client, stream, writer));  // Start a new thread to handle client communication
                clientThread.Start();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while starting the server: {ex.Message}");
        }
        finally
        {
            server?.Stop();
        }
    }

    static void HandleClient(TcpClient client, NetworkStream stream, StreamWriter writer)
    {
        try
        {
            byte[] welcomeMessage = Encoding.ASCII.GetBytes("Welcome to the chat!");
            stream.Write(welcomeMessage, 0, welcomeMessage.Length);  // Send a welcome message to the client

            byte[] usernameBytes = new byte[1024];
            int bytesRead = stream.Read(usernameBytes, 0, usernameBytes.Length);
            string username = Encoding.ASCII.GetString(usernameBytes, 0, bytesRead);  // Receive and decode the client's username

            lock (clientLock)
            {
                foreach (string message in messageHistory)
                {
                    byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                    stream.Write(messageBytes, 0, messageBytes.Length);  // Send message history to the connected client
                }
            }

            while (true)
            {
                byte[] messageBytes = new byte[1024];
                int messageBytesRead = stream.Read(messageBytes, 0, messageBytes.Length);
                string message = Encoding.ASCII.GetString(messageBytes, 0, messageBytesRead);  // Receive and decode client messages

                Console.WriteLine($"Message received from {username}: {message}");

                BroadcastMessage($"{username}: {message}");  // Broadcast the received message to all connected clients
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection error with client: {ex.Message}");
        }
        finally
        {
            lock (clientLock)
            {
                clientWriters.Remove(writer);  // Remove the writer from the list of client writers
            }
            writer.Close();
            stream.Close();
            client.Close();
        }
    }

    static void BroadcastMessage(string message)
    {
        lock (clientLock)
        {
            messageHistory.Add(message);  // Add the message to the chat history

            foreach (var writer in clientWriters)
            {
                try
                {
                    writer.WriteLine(message);  // Send the message to all connected clients
                }
                catch
                {
                    // Ignore errors when sending to a client
                }
            }
        }
    }
}
