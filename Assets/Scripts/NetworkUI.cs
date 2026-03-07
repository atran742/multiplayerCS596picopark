using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

/// <summary>
///  uses ui buttons to start and connect client and host 
/// </summary>
public class NetworkUI : MonoBehaviour
{
    public TMP_InputField ipInput;

    public void StartHost()
    {
        //starts host when player clicks 
        //allows for other clients to join the game 
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        //takes the entered ip address, and starts client 
        //takes care of low-level network connection 
        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        transport.ConnectionData.Address = ipInput.text; //sets the address the client is trying to connect to 

        //connects client to game 
        NetworkManager.Singleton.StartClient();
    }
}