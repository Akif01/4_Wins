using System.Collections.Generic;

namespace Assets.Scripts
{
    public class EventHandlers
    {
        public delegate void EventHandler<T>(T ev) where T : IEvent;

        private readonly List<EventHandler<IEvent>> _handlers = new();

        public void Add<T>(EventHandler<T> handler) where T : IEvent
        {
            void Handle(IEvent ev)
            {
                handler((T)ev);
            }

            _handlers.Add(Handle);
        }

        public IEnumerable<EventHandler<IEvent>> GetHandlers() 
        { 
            return _handlers; 
        }
    }
}
