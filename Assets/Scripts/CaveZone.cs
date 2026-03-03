using UnityEngine;

public class CaveZone : MonoBehaviour
{
    private CameraFollow cameraZoom;
    /// <summary>
    /// just zooms the camera out when the player is in the cave area 
    /// </summary>
    void Start()
    {
        cameraZoom = Camera.main.GetComponent<CameraFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //zooms camera out when player is in cave 
        if (other.CompareTag("Player"))
        {
            cameraZoom.ZoomOut();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    { //zooms back to normal when player is out of the cave 
        if (other.CompareTag("Player"))
        {
            cameraZoom.ZoomIn();
        }
    }
}
