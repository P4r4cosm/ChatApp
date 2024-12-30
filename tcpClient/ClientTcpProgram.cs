﻿using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using ChatDb;
class ClientTcpProgram
{
    static public User CurrentUser { get; set; }
    static async Task SendMessageToServer(string message, NetworkStream stream)
    {
        var response = Encoding.UTF8.GetBytes($"{message}\n");
        await stream.WriteAsync(response);
    }
    static async Task<string> ReadServerMessage(NetworkStream stream, TcpClient client)
    {
        var buffer = new List<byte>();
        int bytesRead;

        // Читаем данные до символа '\n'
        while ((bytesRead = stream.ReadByte()) != -1 && bytesRead != '\n')
        {
            buffer.Add((byte)bytesRead);
        }
        // Обработка сообщения
        var message = Encoding.UTF8.GetString(buffer.ToArray());
        return message;
    }
    static async Task Main()
    {
        using TcpClient tcpClient = new TcpClient();
        Console.WriteLine("Клиент запущен");
        await tcpClient.ConnectAsync("127.0.0.1", 8080);

        if (tcpClient.Connected)
            Console.WriteLine($"Подключение с {tcpClient.Client.RemoteEndPoint} установлено");
        else
        {
            Console.WriteLine("Не удалось подключиться");
            return;
        }
        var stream = tcpClient.GetStream();

        while (true)
        {
            Console.Write("Введите логин: ");
            var login = Console.ReadLine();
            Console.Write("Введите пароль: ");
            var password = Console.ReadLine();
            await SendMessageToServer(login + " " + password, stream);
            var loginResult = ReadServerMessage(stream, tcpClient).Result;
            Console.WriteLine(loginResult);
            if (loginResult == "Login Succesfull")
            {
                CurrentUser = JsonSerializer.Deserialize<User>(ReadServerMessage(stream, tcpClient).Result);
                Console.WriteLine(CurrentUser.ToString());
                break;
            }
        }
        while (true)
        {
            var message = CurrentUser.SendMessage("Привет Настя", 
                new User {Id=4, Name= "Nastysha", Age=20,
                    Password= "t7uRBuLxhoIxBRT/HqsodWVvMeS3D72y8NZdk3PeVO4=", Salt= "L8QeGR+DjLin1L2fXt5n+Q==" });
            await SendMessageToServer(JsonSerializer.Serialize(message), stream);
            await Task.Delay(1000);
            break;
            
        }
    }
}
