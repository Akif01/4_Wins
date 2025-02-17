using Assets.Scripts.Eventing.Events;

namespace Assets.Scripts.Eventing
{
    internal interface IEventPublisher
    {
        public void Publish(IEvent ev);
    }
}
