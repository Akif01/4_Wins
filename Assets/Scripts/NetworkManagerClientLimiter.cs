using UnityEngine;
using Unity.Netcode;

public class NetworkManagerClientLimiter : MonoBehaviour
{
    [SerializeField]
    private int maxPlayers = 2;  // Set your max player count here

    private void Start()
    {
        // Enable connection approval
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        // Check if the max player count has been reached
        if (NetworkManager.Singleton.ConnectedClientsList.Count >= maxPlayers)
        {
            // Reject the connection
            response.Approved = false;
            response.Reason = "Max player count reached";
            Debug.Log("Connection rejected: max player count reached");
        }
        else
        {
            // Approve the connection
            response.Approved = true;
            response.CreatePlayerObject = true; // Set to true if you want to spawn a player object
        }
    }

    private void OnDestroy()
    {
        // Clean up the callback when the object is destroyed
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
    }
}