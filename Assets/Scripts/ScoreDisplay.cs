using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Collections.Generic;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;

    void Update()
    {
        var clients = NetworkManager.Singleton.ConnectedClientsList;
        //loads the players score 
        if (clients.Count > 0 && clients[0].PlayerObject != null)
        {
            PlayerController p1 = clients[0].PlayerObject.GetComponent<PlayerController>();
            player1Text.text = "Player 1: " + p1.gemCount.Value;
        }
        
        
        //loads the players score 
        if (clients.Count > 1 && clients[1].PlayerObject != null)
        {
            PlayerController p2 = clients[1].PlayerObject.GetComponent<PlayerController>();
            player2Text.text = "Player 2: " + p2.gemCount.Value;
        }
    }
}