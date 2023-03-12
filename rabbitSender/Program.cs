using static System.Console;
using RabbitMQ.Client;
using System.Text;

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
        var msg1 = counter++ + " test message - " + DateTime.Now.ToString("HH:mm:ss");
        var body = Encoding.UTF8.GetBytes(msg1);

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
