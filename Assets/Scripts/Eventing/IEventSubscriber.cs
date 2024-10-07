using Assets.Scripts.Eventing.Events;

namespace Assets.Scripts.Eventing
{
    internal interface IEventSubscriber
    {
        public void Subscribe<T>(EventHandlers.EventHandler<T> handler) where T : IEvent;
    }
}