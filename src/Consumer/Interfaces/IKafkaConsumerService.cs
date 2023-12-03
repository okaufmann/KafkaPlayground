namespace Consumer.Interfaces;

public interface IKafkaConsumerService
{
    void StartConsuming(string topic, CancellationToken cancellationToken);
    void StopConsuming();
}