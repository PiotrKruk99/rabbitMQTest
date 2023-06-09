﻿using static System.Console;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using testClass;
using MessagePack;

var factory = new ConnectionFactory()
{
    HostName = "localhost"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("test1", false, false, false, null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var class1 = MessagePackSerializer.Deserialize<TestClass1>(body);
    WriteLine($"Received: {class1.number1} {class1.msg1} -> {DateTime.Now.ToString("HH:mm:ss")}");
};

channel.BasicConsume("test1", true, consumer);
WriteLine("press 'Q' to exit");

do
{
    await Task.Delay(100);
}
while (ReadKey(true).Key != ConsoleKey.Q);

WriteLine("end of receiving");
