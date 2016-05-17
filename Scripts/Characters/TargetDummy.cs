/*
    Target Dummy Data Model

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/
using UnityEngine;
using System.Collections;

public class TargetDummy : Enemy {

    void Awake()
    {
        health = 100;
    }

	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int GetHealth()
    {
        return health;
    }
}
