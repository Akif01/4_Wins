using Assets.Scripts.Eventing.Events;
using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Eventing.Handlers
{
    internal class JoinServerEventHandler : MonoBehaviour
    {
        [Inject]
        private EventBroker _eventBroker;

        private void Start()
        {
            DependencyInjection.InjectDependencies(this);
            _eventBroker.Subscribe<JoinServerEvent>(ev =>
            {
                NetworkManager.Singleton.StartClient();
            });
        }
    }
}
