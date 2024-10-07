using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts.Eventing.Handlers
{
    internal class HostServerEventHandler : MonoBehaviour
    {
        [Inject]
        private EventBroker _eventBroker;

        private void Start()
        {
            DependencyInjection.InjectDependencies(this);
            _eventBroker.Subscribe<HostServerEvent>(ev =>
            {
                NetworkManager.Singleton.StartHost();
            });
        }
    }
}
