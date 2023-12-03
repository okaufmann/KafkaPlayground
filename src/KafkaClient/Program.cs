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

static async Task RunConsumer(IServiceProvider serviceProvider, string topic)
{
    var cancellationTokenSource = new CancellationTokenSource();
    Console.CancelKeyPress += (sender, eventArgs) =>
    {
        eventArgs.Cancel = true; // Prevent the process from terminating.
        cancellationTokenSource.Cancel();
    };
    
    var consumerService = serviceProvider.GetService<IKafkaConsumerService>();
    consumerService.StartConsuming(topic, cancellationTokenSource.Token);
    
    Console.WriteLine("Press ctrl+c key to stop consuming...");
    
    try
    {
        // Block the main thread until cancellation is requested
        await Task.Delay(Timeout.Infinite, cancellationTokenSource.Token);
    }
    catch (TaskCanceledException)
    {
        // Handle any cleanup or finalization here if needed
        Console.WriteLine("Consumer stopping...");
        consumerService.StopConsuming();
    }
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
        await RunConsumer(serviceProvider, topic);
        break;
    default:
        Console.WriteLine("Invalid choice.");
        break;
}