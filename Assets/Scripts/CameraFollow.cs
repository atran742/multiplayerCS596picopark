using UnityEngine;
using Unity.Netcode;

/// <summary>
/// camera follows the midpoint of both players
/// </summary>
public class CameraFollow : MonoBehaviour
{
    //player objects 
    private PlayerController player1;
    private PlayerController player2;
    //sets camera bounds 
    public float deadZoneWidth = 4f;
    public float deadZoneHeight = 3f;
    //camera smoothness 
    public float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;
    //zoom setting 
    public float normalSize = 5f;
    public float zoomedOutSize = 8f;
    public float zoomSpeed = 5f;

    private Camera cam;
    private float targetSize;
    //initializes camera 
    void Start()
    {
        cam = GetComponent<Camera>();
        targetSize = normalSize;

        InvokeRepeating(nameof(FindPlayers), 0.5f, 0.5f);
    }

    void Update()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, zoomSpeed * Time.deltaTime);

        //if either player is missing, try to find them 
        if (player1 == null || player2 == null)
        {
            FindPlayers();
        }
    }

    void LateUpdate()
    { // make camera follow the midpoint between the players 
        if (player1 == null || player2 == null)
            return;

        //gets the midpoint between the players 
        Vector3 midpoint = (player1.transform.position + player2.transform.position) / 2f;

        Vector3 currentPos = transform.position;
        Vector3 targetPos = currentPos;

        if (Mathf.Abs(midpoint.x - currentPos.x) > deadZoneWidth / 2f)
            targetPos.x = midpoint.x;

        if (Mathf.Abs(midpoint.y - currentPos.y) > deadZoneHeight / 2f)
            targetPos.y = midpoint.y;

        targetPos.z = -10f;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
    /// <summary>
    /// Attempts to locate the first two connected players
    /// from the NetworkManager.
    /// 
    /// only reads network data and does not modify it.
    /// </summary>
    void FindPlayers()
    {
        if (NetworkManager.Singleton == null)
            return;

        if (!NetworkManager.Singleton.IsListening)
            return;

        var clients = NetworkManager.Singleton.ConnectedClientsList;

        //only runs if there are two players 
        if (clients.Count >= 2)
        {
            if (clients[0].PlayerObject != null && clients[1].PlayerObject != null)
            {
                player1 = clients[0].PlayerObject.GetComponent<PlayerController>();
                player2 = clients[1].PlayerObject.GetComponent<PlayerController>();
            }
        }
    }
    //used in cave zone script 
    public void ZoomOut()
    {
        targetSize = zoomedOutSize;
    }
    //used in cave zone script 
    public void ZoomIn()
    {
        targetSize = normalSize;
    }
}