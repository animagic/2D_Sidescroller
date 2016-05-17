/*
    State Machine for the Player.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System.Collections;

public class ProjectileData : MonoBehaviour {

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Rigidbody2D enemy;

    [SerializeField]
    private int damage;
    [SerializeField]
    private float speed = 25f;
    [SerializeField]
    private float direction;

	// Use this for initialization
	void Start () {
        SetProjectileStats();
        Destroy(gameObject, 2f);
        player = GameObject.Find("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

    }

    /*
        Fires when the projectile hits an object with a Collider on it.  If the Collider is not the player,
        it simply kills the projectile.  If the Collider is an Enemy, it fires the enemy's TakeDamage()
        method and destroys the projectile.  In the special case of the TargetDummy, the TakeDamage() method
        uses 0 damage since we do not want to kill the dummy.  It also fires the dummy's state machine then
        destroys the game object.  In the case of a more advanced game, like an RPG, where player traits and
        skills might alter depending on the enemy's health, we would need to get damage calculations
        from the enemy and make adjustments on the projectile's damage stat from the player's bonuses for 
        the TakeDamage method.
    */
    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.transform.tag != "Player")
        {
            if (col.transform.tag == "Enemy")
            {
                if (col.gameObject.name == "TargetDummy")
                {
                    TargetDummyStateMachine dummy = col.transform.GetComponent<TargetDummyStateMachine>();
                    dummy.SetTargetHit();
                    col.transform.GetComponentInParent<Enemy>().TakeDamage(0);
                    Destroy(gameObject);
                }
                else
                {
                    col.transform.GetComponentInParent<Enemy>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }     
        }
        else if (col.transform.tag == "Player")
        {
            player.GetComponent<Player>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void SetProjectileStats()
    {
        damage = 20;
    }

    public float GetProjectileSpeed()
    {
        return speed;
    }

    public int GetProjectileDamage()
    {
        return damage;
    }
}
