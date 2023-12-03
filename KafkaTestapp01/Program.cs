// See https://aka.ms/new-console-template for more information

using KafkaTestapp01.Example01;

string bootstrapServers = "localhost:9092"; // Replace with your Kafka broker address
string topic = "test-topic"; // Replace with your topic

// Run the producer
await KafkaProducer.Produce(topic, bootstrapServers);

// Run the consumer
Task.Run(() => KafkaConsumer.Consume(topic, bootstrapServers, "my-consumer-group"));
    
Console.WriteLine("Press any key to exit");
Console.ReadKey();