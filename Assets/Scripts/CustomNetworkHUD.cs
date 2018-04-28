using UnityEngine.Networking;
using UnityEngine;

[AddComponentMenu("Network/CustomNetworkHUD")]
[RequireComponent(typeof(NetworkManager))]

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]

public class CustomNetworkHUD : MonoBehaviour
{
    public NetworkManager manager;
    
    /// <summary>
    /// Run on startup
    /// </summary>
    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    /// <summary>
    /// Event Listener, start game as host
    /// </summary>
    public void HostGame()
    {
        manager.StartHost();
    }

    /// <summary>
    /// Event Listener, join game as client
    /// </summary>
    public void JoinGame()
    {
        manager.StartClient();
    }

    /// <summary>
    /// End game, host only, equivalent to stopclient when the host
    /// </summary>
    public void StopHost()
    {
        manager.StopHost();
    }

    /// <summary>
    /// Leave game as client
    /// </summary>
    public void StopClient()
    {
        manager.StopClient();
    }
}