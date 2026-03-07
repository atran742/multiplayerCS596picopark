using UnityEngine;
using Unity.Netcode;

public class TrapDoor : NetworkBehaviour
{
    /// <summary>
    /// allows for a game object trap door to slide open when the other player steps on the pressure plate 
    /// </summary>
    public NetworkVariable<bool> isOpen = new NetworkVariable<bool>(false);

    private Collider2D col;
    private Vector3 closedPosition;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        closedPosition = transform.position;
    }

    public override void OnNetworkSpawn()
    {
        isOpen.OnValueChanged += OnTrapStateChanged;
        ApplyState(isOpen.Value);
    }

    void OnTrapStateChanged(bool previous, bool current)
    {
        ApplyState(current);
    }

    void ApplyState(bool open)
    {
        col.enabled = !open; // disable collider when open
        transform.position = open
            ? closedPosition + new Vector3(-5f, 0f, 0f)
            : closedPosition;
    }

    [ServerRpc(RequireOwnership = false)]
    public void OpenTrapServerRpc()
    {
        isOpen.Value = true;
    }
}