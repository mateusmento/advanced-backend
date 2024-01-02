using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);

    var queueName = channel.QueueDeclare().QueueName;

    if(args.Length < 1)
    {
        Console.Error.WriteLine("Usage: {0} [info] [warning] [error]", Environment.GetCommandLineArgs()[0]);
        Environment.ExitCode = 1;
        return;
    }

    foreach (var severity in args)
        channel.QueueBind(
            queue: queueName,
            exchange: "direct_logs",
            routingKey: severity
        );

    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var routingKey = ea.RoutingKey;
        Console.WriteLine(" [x] Received '{0}':'{1}'", routingKey, message);

        int dots = message.Split('.').Length - 1;
        Thread.Sleep(dots * 1000);

        Console.WriteLine(" [x] Done");
        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    };

    channel.BasicConsume(
        queue: queueName,
        autoAck: false,
        consumer: consumer
    );

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}
