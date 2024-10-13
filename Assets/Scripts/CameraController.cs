using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : NetworkBehaviour
    {
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            gameObject.GetComponentInChildren<Camera>().enabled = IsOwner;
            gameObject.GetComponentInChildren<AudioListener>().enabled = IsOwner;
        }
    }
}
