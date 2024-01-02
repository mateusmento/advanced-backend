using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var conn = factory.CreateConnection())
using (var channel = conn.CreateModel())
{
    // channel.ExchangeDeclare(exchange: "direct_logs", ExchangeType.Direct);

    var severity = GetSeverity(args);
    var message = GetMessage(args);
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(
        exchange: "direct_logs",
        routingKey: severity,
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

string GetSeverity(string[] args)
{
    return args.Length > 0 ? args[0] : "info";
}