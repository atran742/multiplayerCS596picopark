using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

public class Door : NetworkBehaviour
{
    [SerializeField] private string sceneToLoad;
    
    /// <summary>
    /// brings players to the next scene if they touch the door 
    /// </summary>

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if the code is not running on the server, don't execute anything 
        if (!IsServer) return; 

        //checks if both players are on the door 
        if (!other.TryGetComponent<PlayerController>(out var player))
            return;

        LoadNextLevel();
    }
    
    void LoadNextLevel()
    {   //gets the possible next level/ end scene 
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            return;
        }
        //loads the next scene 
        NetworkManager.SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);

    }
}