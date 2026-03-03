using UnityEngine;
using Unity.Netcode;
/// <summary>
/// creates the gems, when the player touches the gem, the gem disappears and then adds to the players score 
/// </summary>
public class KeyScript : NetworkBehaviour
{
    public NetworkVariable<bool> isCollected = new NetworkVariable<bool>(false);

    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public override void OnNetworkSpawn()
    {
        isCollected.OnValueChanged += OnCollectedChanged; //for score counter, counts the amount of gems a players has 
        ApplyState(isCollected.Value);
    }

    void OnCollectedChanged(bool previous, bool current)
    {
        ApplyState(current);
    }

    void ApplyState(bool collected)
    {
        spriteRenderer.enabled = !collected;
        col.enabled = !collected; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;
        // if the player touches the gem, then the gem should disappear 
        if (collision.CompareTag("Player") && !isCollected.Value)
        {
            NetworkObject playerNetObj = collision.GetComponent<NetworkObject>();
            if (playerNetObj != null)
            {
                Pickup(playerNetObj);
            }
        }
    }

    void Pickup(NetworkObject player)
    {
        PlayerController pc = player.GetComponent<PlayerController>();
        if (pc != null)
        {
            pc.gemCount.Value += 1; // increases the players score 
        }

        isCollected.Value = true;
    }
    
}