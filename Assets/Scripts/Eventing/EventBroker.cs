using Assets.Scripts.Eventing.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Eventing
{
    public class EventBroker : IEventPublisher, IEventSubscriber
    {
        private readonly Dictionary<Type, EventHandlers> _eventHandlersOfType = new();

        public void Subscribe<T>(EventHandlers.EventHandler<T> handler) where T : IEvent
        {
            if (!_eventHandlersOfType.TryGetValue(typeof(T), out var handlers))
            {
                handlers = new EventHandlers();
                _eventHandlersOfType.Add(typeof(T), handlers);
            }

            handlers.Add(handler);
        }

        public void Publish(IEvent ev)
        {
            var eventType = ev.GetType();

            if (_eventHandlersOfType.TryGetValue(eventType, out var handlers))
            {
                foreach (var handler in handlers.GetHandlers().ToList())
                {
                    handler(ev);
                }
            }
            else
            {
                throw new Exception($"No handlers for event of type {eventType.Name}!");
            }
        }
    }
}