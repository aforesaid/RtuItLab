namespace WebRabbitMQ
{
    public interface IEventBus
    {
        void Publish(string message);
        void Subscribe();
    }
}
