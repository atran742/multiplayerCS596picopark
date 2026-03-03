using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using System.Linq;

/// <summary>
/// responsible for positioning all connected players at predefined spawn points after a multiplayer scene loads.
///
/// This runs only on the server to maintain authoritative control
/// over player positioning.
/// </summary>
public class SceneSpawnManager : NetworkBehaviour
{
    public Transform[] spawnPoints;

    //loads next scene if running on server 
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += OnSceneLoaded;
        }
    }
    //calls PositionPlayers()
    private void OnSceneLoaded(string sceneName, LoadSceneMode mode, System.Collections.Generic.List<ulong> clientsCompleted, System.Collections.Generic.List<ulong> clientsTimedOut)
    {
        PositionPlayers();
    }
    //ensures that players are spawned back at their correct positions 
    void PositionPlayers()
    {
        var players = NetworkManager.Singleton.ConnectedClients
            .Select(c => c.Value.PlayerObject)
            .Where(p => p != null)
            .ToList();

        for (int i = 0; i < players.Count && i < spawnPoints.Length; i++)
        {
            players[i].transform.position = spawnPoints[i].position;
        }
    }
}