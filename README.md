# Chat Application with Word Prediction

This repository contains a simple chat application implemented in C# using the TCP/IP protocol for communication between a server and clients. The chat application features a basic word prediction feature based on word frequency analysis.

## Features

- **Server:** The `ChatServer` class sets up a server that listens for incoming client connections. It allows multiple clients to connect and exchange messages.

- **Client:** The `ChatClient` class enables clients to connect to the server and participate in the chat. It includes a basic word prediction feature that suggests the next word based on the input context.

- **WordPrediction:** The `WordPrediction` class provides the word prediction logic. It uses a frequency-based approach to predict the next word given the current input context.

- **ConsoleInputWithNavigation:** The `ConsoleInputWithNavigation` class allows clients to input text in the console with navigation capabilities, including word prediction suggestions.

## How to Use

1. **Server Setup:** Compile and run the `ChatServer` class to start the chat server. Provide the IPv4 address where the server should listen for connections.

2. **Client Connection:** Compile and run the `ChatClient` class to connect to the server. Enter the server's IPv4 address. Enter your username when prompted.

3. **Chatting:** Once connected, you can send and receive messages between clients. The word prediction feature provides suggestions for the next word while typing.

## File Descriptions

- `ChatServer.cs`: Contains the implementation of the chat server, allowing multiple clients to connect and exchange messages.

- `ChatClient.cs`: Implements the chat client, enabling connection to the server and participation in the chat with word prediction.

- `WordPrediction.cs`: Implements the word prediction logic, predicting the next word based on frequency analysis.

- `ConsoleInputWithNavigation.cs`: Provides console input with navigation capabilities, including word prediction suggestions.

## Dependencies

- The project relies on the C# standard libraries and does not require additional external dependencies.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## About the Author

This project was developed by Fabian Müller.

---

Feel free to contribute to the project by submitting pull requests or opening issues. For questions or further information, contact [Fabian Müller's contact information here].
