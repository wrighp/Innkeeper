using UnityEngine.Networking;
using UnityEngine;

[AddComponentMenu("Network/CustomNetworkHUD")]
[RequireComponent(typeof(NetworkManager))]

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

public class CustomNetworkHUD : MonoBehaviour
{
    public NetworkManager manager;

    // Runtime variable
    bool showServer = false;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void HostGame() {
        manager.StartHost();
    }

    public void JoinGame() {
        manager.StartClient();
    }

    public void StopHost() {
        manager.StopHost();
    }

    public void StopClient() {
        manager.StopClient();
    }
}