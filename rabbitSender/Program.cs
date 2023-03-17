using static System.Console;
using RabbitMQ.Client;
using testClass;
using MessagePack;

var factory = new ConnectionFactory()
{
    HostName = "localhost"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare("test1", false, false, false, null);

WriteLine("press 'Q' if you want to quit and 'P' if you want to pause"
            + Environment.NewLine);

var keyPressed = ConsoleKey.Spacebar;
var paused = false;
var counter = 1;

do
{
    if (!paused)
    {
        TestClass1 class1 = new TestClass1()
        {
            msg1 = "test message - " + DateTime.Now.ToString("HH:mm:ss"),
            number1 = counter++
        };

        var body = MessagePackSerializer.Serialize(class1);
        channel.BasicPublish(string.Empty, "test1", null, body);
    }

    await Task.Delay(900);

    if (KeyAvailable)
        keyPressed = ReadKey(true).Key;

    if (keyPressed == ConsoleKey.P)
    {
        paused = !paused;
        keyPressed = ConsoleKey.Spacebar;

        if (paused)
            WriteLine("sending paused");
        else
            WriteLine("sending resumed");
    }
}
while (keyPressed != ConsoleKey.Q);

WriteLine(Environment.NewLine + "end of sending");
