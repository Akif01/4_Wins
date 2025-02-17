using UnityEngine;

namespace Assets.Scripts.Eventing.Publishers
{
    public class HostServerEventPublisher : MonoBehaviour
    {
        [Inject]
        private EventBroker _eventBroker;

        private void Start()
        {
            DependencyInjection.InjectDependencies(this);
        }

        public void PublishHostServerEvent()
        {
            _eventBroker.Publish(new HostServerEvent());
        }
    }
}
