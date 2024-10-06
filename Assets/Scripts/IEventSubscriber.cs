namespace Assets.Scripts
{
    internal interface IEventSubscriber
    {
        public void Subscribe<T>(EventHandlers.EventHandler<T> handler) where T : IEvent;
    }
}