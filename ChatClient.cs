using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatClient
{
    public static string currentInput = "";

    static void Main(string[] args)
    {
        Console.WriteLine("Enter the server IP address (IPv4):");

        try
        {
            string serverIpAddress = Console.ReadLine();
            TcpClient client = new TcpClient(serverIpAddress, 8888);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.ASCII);
            StreamWriter writer = new StreamWriter(stream, Encoding.ASCII) { AutoFlush = true };

            // Receive the welcome message from the server
            byte[] welcomeMessageBytes = new byte[1024];
            int welcomeMessageBytesRead = stream.Read(welcomeMessageBytes, 0, welcomeMessageBytes.Length);
            string welcomeMessage = Encoding.ASCII.GetString(welcomeMessageBytes, 0, welcomeMessageBytesRead);
            Console.WriteLine($"Server: {welcomeMessage}");

            Console.Write("Please enter your username: ");
            string username = Console.ReadLine();
            byte[] usernameBytes = Encoding.ASCII.GetBytes(username);
            stream.Write(usernameBytes, 0, usernameBytes.Length);

            // Start a thread to receive incoming messages
            Thread receiveThread = new Thread(() => ReceiveMessages(reader));
            receiveThread.Start();

            // Loop to send messages
            while (true)
            {
                ConsoleInputWithNavigation inputHandler = new ConsoleInputWithNavigation();
                string input = inputHandler.GetInput();
                string message = input;
                byte[] messageBytes = Encoding.ASCII.GetBytes(message);
                stream.Write(messageBytes, 0, messageBytes.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public static void UpdateWindowTitle()
    {
        WordPrediction wordPrediction = new WordPrediction();
        string[] text = currentInput.Split(" ");
        Console.Title = wordPrediction.PredictNextWord(text[text.Length - 1]);
    }

    static void ReceiveMessages(StreamReader reader)
    {
        try
        {
            while (true)
            {
                string message = reader.ReadLine();
                Console.WriteLine(message);
            }
        }
        catch
        {
            // End the receiving thread in case of connection problems
        }
    }
}
