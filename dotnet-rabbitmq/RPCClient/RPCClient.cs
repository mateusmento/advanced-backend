using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Collections.Concurrent;

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var conn = factory.CreateConnection())
using (var channel = conn.CreateModel())
{
    var queueName = channel.QueueDeclare().QueueName;
    var corrolationId = Guid.NewGuid().ToString();
    var replyQueue = new BlockingCollection<string>();

    var consumer = new EventingBasicConsumer(channel);
    consumer.Received += (model, ea) =>
    {
        var props = ea.BasicProperties;
        if (props.CorrelationId == corrolationId)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            replyQueue.Add(message);
        }
    };

    channel.BasicConsume(
        queue: queueName,
        autoAck: true,
        consumer: consumer
    );

    var props = channel.CreateBasicProperties();
    props.ReplyTo = queueName;
    props.CorrelationId = corrolationId;

    Console.WriteLine(" [x] Requesting fib(30)");
    var body = Encoding.UTF8.GetBytes("30");

    channel.BasicPublish(
        exchange: "",
        routingKey: "rpc_queue",
        basicProperties: props,
        body: body
    );

    var response = replyQueue.Take();
    Console.WriteLine(" [.] Got '{0}'", response);
}

