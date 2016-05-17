using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Recall : MonoBehaviour {

    [SerializeField]
    private Player player;
    private Rigidbody2D playerRB;
    [SerializeField]
    private int hp;

    private List<Vector2> locationArray;
    private List<int> healthArray;
    [SerializeField]
    private int arrayIndex;


    [SerializeField]
    private bool recalling;

    void Awake()
    {
        
    }

	// Use this for initialization
	void Start () {

        locationArray = new List<Vector2>();
        healthArray = new List<int>();
        hp = player.GetHealth();
        player = GetComponent<Player>();
        playerRB = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {
	
        if (!recalling)
        {
            locationArray.Add(playerRB.GetComponent<Transform>().position);
            healthArray.Add(player.GetHealth());
            arrayIndex++;
        }
        
        if (arrayIndex != locationArray.Count)
        {
            arrayIndex = locationArray.Count;
        }

        if (Input.GetKey(KeyCode.E))
        {
            recalling = true;
            Recalling();
        }
        else
        {
            recalling = false;
        }
	}

    void Recalling()
    {
        arrayIndex--;
        Debug.Log(locationArray[arrayIndex]);
        //transform.position = new Vector3(locationArray[arrayIndex]);

    }

}
