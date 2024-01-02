using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var conn = factory.CreateConnection())
using (var channel = conn.CreateModel())
{
    channel.ExchangeDeclare(exchange: "topic_logs", ExchangeType.Topic);

    var route = GetRoute(args);
    var message = GetMessage(args);
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "topic_logs",
        routingKey: route,
        basicProperties: null,
        body: body
    );
}

string GetMessage(string[] args)
{
    return (args.Length > 1)
        ? string.Join(" ", args.Skip( 1 ).ToArray())
        : "Hello World!";
}

string GetRoute(string[] args)
{
    return args.Length > 0 ? args[0] : "default.route";
}
