/*
    Small script to let the player know that the level has been won.

    Author: Wayne Work
    Date: 3/13/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WinGame : MonoBehaviour {

    [SerializeField]
    private Player player;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            // Finds all of the Renderer objects in all children of the YouWin object and turns their material to green.
            foreach (Renderer rend in GameObject.Find("YouWin").GetComponentsInChildren<Renderer>())
            {
                rend.material.color = Color.green;
            }
            
            // Fires Restart() after 1.5 seconds.
            Invoke("Restart", 1.5f);
        }
    }

    // RestartScene can be used if this was a Build but since we are not building the game, the other Restart is used
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }

    void Restart()
    {
        // Finds all of the Renderer objects in all children of the YouWin object and turns their material to white.
        foreach (Renderer rend in GameObject.Find("YouWin").GetComponentsInChildren<Renderer>())
        {
            rend.material.color = Color.white;
        }

        Debug.Log("Congratulations! You have escaped the underground cave!");
        player.GetComponent<Rigidbody2D>().GetComponent<Transform>().position = GameObject.Find("SpawnPoint").GetComponent<Transform>().position;
        player.SetPlayerHealth(100);
    }
}
