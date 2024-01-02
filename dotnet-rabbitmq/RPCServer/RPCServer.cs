using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{
    channel.QueueDeclare(
        queue: "rpc_queue",
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null
    );

    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine(" [.] fib({0})", message);

        string res = int.TryParse(message, out int n)
            ? fib(n).ToString()
            : "";

        var props = ea.BasicProperties;
        var replyProps = channel.CreateBasicProperties();
        replyProps.CorrelationId = props.CorrelationId;
        var replayQueue = props.ReplyTo;

        channel.BasicPublish(
            exchange: "",
            routingKey: replayQueue,
            basicProperties: replyProps,
            body: Encoding.UTF8.GetBytes(res)
        );

        channel.BasicAck(ea.DeliveryTag, false);
    };

    channel.BasicConsume(
        queue: "rpc_queue",
        autoAck: false,
        consumer: consumer
    );

    Console.WriteLine(" [x] Awaiting RPC requests");

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}



int fib(int n) => 
    (n == 0 || n == 1) ? n : fib(n - 1) + fib(n - 2);