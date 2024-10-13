using Unity.Netcode;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerPositionController : NetworkBehaviour
    {
        [SerializeField]
        private Transform[] _spawnPoints = new Transform[2]; // Array of possible spawn points

        [SerializeField]
        private Transform _playerTransform;

        private void Start()
        {
            _playerTransform ??= gameObject.transform;
            SetPosition();
        }

        private void SetPosition()
        {
            if (!IsOwner)
                return;

            var clientId = NetworkManager.Singleton.LocalClientId;

            // Choose the correct spawn point
            Transform clientSpawnPoint = _spawnPoints[clientId > (ulong)_spawnPoints.Length ? (ulong)_spawnPoints.Length - 1 : clientId];

            // Get the player object
            var player = gameObject;

            // Set the player position on the server
            player.transform.position = clientSpawnPoint.position;
            player.transform.rotation = clientSpawnPoint.rotation;
        }
    }
}
