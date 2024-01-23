// See https://aka.ms/new-console-template for more information


using Confluent.Kafka;
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
        var text = Console.ReadLine();
        if (string.IsNullOrEmpty(text)) continue;


        if (text.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
        await producerService.ProduceAsync(topic, "message", text);
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

var serviceCollection = new ServiceCollection();

var serviceProvider = serviceCollection.AddSingleton<IProducer<string, string>>(provider =>
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:19092" };
        return new ProducerBuilder<string, string>(config).Build();
    })
    .AddSingleton<IKafkaProducerService, KafkaProducerService>()
    .AddSingleton<IConsumer<string, string>>(consumer =>
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:19092",
            GroupId = "my-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
        return new ConsumerBuilder<string, string>(config).Build();
    })
    .AddSingleton<IKafkaConsumerService, KafkaConsumerService>()
    .BuildServiceProvider();

Console.WriteLine("Choose mode: 1 for Producer, 2 for Consumer");
var choice = Console.ReadLine();

var topic = "test-topic"; // Replace with your topic
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