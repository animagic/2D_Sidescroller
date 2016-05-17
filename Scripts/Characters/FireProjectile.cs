/*
    Common Projectile Attack script.  I wanted to use this in place of directly placing the
    Attack method in every enemy type, but I could not get it to reference correctly.  More 
    time is needed for bug testing.  Leaving in the project for future use.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System.Collections;

public class FireProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Attack(float speed, Vector3 direction)
    {
        GameObject clone = (GameObject)Instantiate(Resources.Load("Projectile"), transform.position, transform.rotation) as GameObject;
        clone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(direction * speed);
    }
}
