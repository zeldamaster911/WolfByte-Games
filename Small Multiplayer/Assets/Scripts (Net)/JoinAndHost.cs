using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.UIElements;

public class JoinAndHost : MonoBehaviour
{
    // Public variable to store the player's network index
    UnityTransport transport;
    private void Start()
    {
        // Set the network transport address and port
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.SetConnectionData(transform.FindDeepChild("IP").GetComponent<TMP_InputField>().text, ushort.Parse( transform.FindDeepChild("Port").GetComponent<TMP_InputField>().text));
    }

    // Method to start the game as host
    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        transport.SetConnectionData(transform.FindDeepChild("IP").GetComponent<TMP_InputField>().text, ushort.Parse(transform.FindDeepChild("Port").GetComponent<TMP_InputField>().text));
    }

    // Method to start the game as client
    public void JoinGame()
    {
        NetworkManager.Singleton.StartClient();
        transport.SetConnectionData(transform.FindDeepChild("IP").GetComponent<TMP_InputField>().text, ushort.Parse(transform.FindDeepChild("Port").GetComponent<TMP_InputField>().text));
    }

    // Method to be called when the network object spawns
    
}
