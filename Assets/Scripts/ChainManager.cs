using UnityEngine;
using Unity.Netcode;
using System.Linq;

[RequireComponent(typeof(LineRenderer))]

///creates a rope which connects the two players 
public class ChainManager : NetworkBehaviour
{
    public float maxDistance = 7f;

    private Rigidbody2D player1;
    private Rigidbody2D player2;

    private LineRenderer lr;

    private bool jointCreated = false;

    public override void OnNetworkSpawn()
    {
        lr = GetComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.useWorldSpace = true; // loads in the line renderer
    }

    void Update()
    {
        if (!jointCreated)
        {
            TryFindPlayersAndCreateJoint();
        }
        // will only spawn the line renderder if there are two players
        if (player1 && player2)
        {
            lr.SetPosition(0, player1.position);
            lr.SetPosition(1, player2.position);
        }
    }
    //checks if there are two players present and creates the rope 
    void TryFindPlayersAndCreateJoint()
    {
        var players = NetworkManager.Singleton.ConnectedClients.Select(c => c.Value.PlayerObject).Where(p => p != null).ToList();

        if (players.Count < 2)
            return;

        player1 = players[0].GetComponent<Rigidbody2D>();
        player2 = players[1].GetComponent<Rigidbody2D>();

        if (player1 && player2)
        {
            CreateJoint();
            jointCreated = true;
        }
    }

    void CreateJoint()
    {
        //creates the rope that connects the players
        DistanceJoint2D joint = player1.gameObject.AddComponent<DistanceJoint2D>();
        joint.connectedBody = player2;
        joint.autoConfigureDistance = false;
        joint.maxDistanceOnly = true;
        joint.distance = maxDistance;
    }
}

