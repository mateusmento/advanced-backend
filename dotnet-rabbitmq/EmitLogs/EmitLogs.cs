using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory() { HostName = "localhost" };
using var conn = factory.CreateConnection();
using var channel = conn.CreateModel();

channel.ExchangeDeclare(exchange: "logs", ExchangeType.Fanout);

var message = GetMessage(args);
var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(
    exchange: "logs",
    routingKey: "",
    basicProperties: null,
    body: body
);

string GetMessage(string[] args)
{
    return args.Length > 0 ? string.Join(" ", args) : "Hello World!";
}
