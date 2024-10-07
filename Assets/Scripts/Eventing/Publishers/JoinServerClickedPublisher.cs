using Assets.Scripts.Eventing.Events;
using UnityEngine;

namespace Assets.Scripts.Eventing.Publishers
{
    public class JoinServerEventPublisher : MonoBehaviour
    {
        [Inject]
        private EventBroker _eventBroker;

        private void Start()
        {
            DependencyInjection.InjectDependencies(this);
        }

        public void PublishJoinServerEvent()
        {
            _eventBroker.Publish(new JoinServerEvent());
        }
    }
}
