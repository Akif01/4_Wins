using UnityEngine;

namespace Assets.Scripts
{
    internal class HostServerClickedHandler : MonoBehaviour
    {
        [Inject]
        private EventBroker _eventBroker;

        private void Start() 
        {
            DependencyInjection.InjectDependencies(this);
        }

        public void PublishHostServerClickedEvent()
        {
            _eventBroker.Publish(new HostServerClickedEvent());
        }

    }

    public class HostServerClickedEvent : IEvent { };
}
