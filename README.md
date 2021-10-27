# ChatApplication
The goal of this exercise is to create a simple browser-based chat application using .NET

This application should allow several users to talk in a chatroom and also to get stock quotesfrom an API using a specific command.

This solution has 7 projects:

1) ChatApplication: This is an MVC web project that contains the front end of the chat application.
2) ChatApplication.Bot: This is the decoupled bot, its function is to read and send messages through RabbitMQ client.
3) ChatApplication.Bot.Test: This is a xUnit project, this project has some tests for ChatApplication.Bot functionality.
4) ChatApplication.Domain: This project has chat application logic.
5) ChatApplication.Domain.Entities: All the entities needed are here.
6) ChatApplication.Infrastructure: Here is the database connection and interaction.
7) ChatApplication.Installer: This project lets create a chatbot installer.

Pre-requisites:

1) RabbitMQ: https://www.rabbitmq.com/download.html
2) .Net 5.0: https://dotnet.microsoft.com/download/dotnet/5.0

Instructions:

Clone this repository and restore all the dependencies in each project.

Set ChatApplication project as the startup project.

The database is already created, is an SQLite database and its name is Chat.db, in this database are the main configurations to get started.

To run the chatbot, use the installer located in ChatApplication/ChatApplication.Bot.Installer/setup.exe.





