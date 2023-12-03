// See https://aka.ms/new-console-template for more information


using Consumer.Interfaces;
using Consumer.Services;
using Microsoft.Extensions.DependencyInjection;
using Producer.Interfaces;
using Producer.Services;

static async Task RunProducer(IServiceProvider serviceProvider, string topic)
{
    var producerService = serviceProvider.GetService<IKafkaProducerService>();
    Console.WriteLine("Enter your messages (type 'exit' to quit):");
    while (true)
    {
        string text = Console.ReadLine();
        if (text.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
        await producerService.ProduceAsync(topic, text);
    }
}

static void RunConsumer(IServiceProvider serviceProvider, string topic)
{
    var cancellationTokenSource = new CancellationTokenSource();
    var consumerService = serviceProvider.GetService<IKafkaConsumerService>();
    consumerService.StartConsuming(topic, cancellationTokenSource.Token);

    Console.WriteLine("Press any key to stop consuming...");
    Console.ReadKey();
    cancellationTokenSource.Cancel();
    consumerService.StopConsuming();
}

var serviceProvider = new ServiceCollection()
    .AddSingleton<IKafkaProducerService, KafkaProducerService>(_ =>
        new KafkaProducerService("localhost:9092"))
    .AddSingleton<IKafkaConsumerService, KafkaConsumerService>(_ =>
        new KafkaConsumerService("localhost:9092", "my-consumer-group"))
    .BuildServiceProvider();

Console.WriteLine("Choose mode: 1 for Producer, 2 for Consumer");
var choice = Console.ReadLine();

string topic = "test-topic"; // Replace with your topic
switch (choice)
{
    case "1":
        await RunProducer(serviceProvider, topic);
        break;
    case "2":
        RunConsumer(serviceProvider, topic);
        break;
    default:
        Console.WriteLine("Invalid choice.");
        break;
}