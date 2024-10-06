namespace Assets.Scripts
{
    internal interface IEventPublisher
    {
        public void Publish(IEvent ev);
    }
}
