using UnityEngine;
using Unity.Netcode;

public class PressurePlate : NetworkBehaviour
{
    /// <summary>
    /// creates a pressure plate, when the player stands on the pressure plate, the trap door will open 
    /// </summary>
    [SerializeField] private TrapDoor trapDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;
        //if the player touches the pressure plate the trap door should open
        if (collision.CompareTag("Player"))
        {
            trapDoor.OpenTrapServerRpc();
        }
    }
}