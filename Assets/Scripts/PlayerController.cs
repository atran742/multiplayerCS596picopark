using UnityEngine;
using Unity.Netcode;

public class PlayerController : NetworkBehaviour
{
    //movement setting 
    public float speed = 5f;
    public float jumpForce = 7f;

    //score count 
    public NetworkVariable<int> gemCount = new NetworkVariable<int>(0);
    
    private float moveInput;
    private Rigidbody2D rb;
    private bool isGrounded;
    
    //for checking the ground 
    [SerializeField] private Transform groundCheck; 
    [SerializeField] private float groundCheckRadius = 0.2f; 
    [SerializeField] private LayerMask groundLayer; 
    [SerializeField] private LayerMask player; 
    
    private Vector3 spawnPoint;
    
    private float lastInput;

    /// <summary>
    /// The script holds players movement, jumping/ detecting the ground, and safe respawning with
    /// netcode RPCs
    ///
    /// Everything is owner-authoritative, own the ones owning their client or host can control their player 
    /// </summary>

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spawnPoint = transform.position; //stores initial spawn point 
    }

    void Update()
    {
       
        if (!IsOwner) return; // if not the owner, don't countine 
        
        //for flipping the character animation 
        float movDir= Input.GetAxisRaw("Horizontal");
        if (movDir > 0) {
            transform.localScale = new Vector3(1, 1, 1); //Face right
        }
        else if (movDir < 0) { 
            transform.localScale = new Vector3(-1, 1, 1); //Face left
        }
        
        //Arrow key movement
        moveInput = 0f; 
        if (Input.GetKey(KeyCode.LeftArrow))
            moveInput = -1f; 
        if (Input.GetKey(KeyCode.RightArrow))
            moveInput = 1f;
        
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y); 
        //Up arrow jump
        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        } 
    }
    
    void FixedUpdate()
        {
            //checks if jump can occur 
            isGrounded = Physics2D.OverlapCircle( groundCheck.position, groundCheckRadius, groundLayer | player );
        }

    //detects if the player enters the kill zone/ dies, only the owning client can request to respawn
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsOwner) return;
        //if a plauer falls off the platform, they should respawn back to the beginning
        if (collision.CompareTag("KillZone"))
        {
            RequestRespawnServerRpc();
        }
    }
    
    /// <summary>
    /// Sent from the owning client to the server.
    /// The server is authoritative and decides which client
    /// should respawn. This prevents clients from cheating
    /// by teleporting themselves.
    /// </summary>
    [ServerRpc]
    void RequestRespawnServerRpc(ServerRpcParams rpcParams = default)
    {
        // tells all clients which player should be respawned
        RespawnClientRpc(rpcParams.Receive.SenderClientId);
    }
    
    /// <summary>
    /// Called by the server and executed on ALL clients.
    /// Only the correct client (matching clientId)
    /// will perform the actual respawn.
    /// </summary>
    
    [ClientRpc]
    void RespawnClientRpc(ulong clientId)
    {
        //only the correct client resets its position
        if (NetworkManager.Singleton.LocalClientId != clientId)
            return;
        //gets the spawn points 
        transform.position = spawnPoint;
        rb.linearVelocity = Vector2.zero;
    }
}