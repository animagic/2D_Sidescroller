/*
    Base Enemy Model for all Characters in the game.  Derives from Character.
    We Initialize all Enemy specifc variables as protected so that only derivatives of this class
    can access them.  Common methods are defined here for all enemy types to use.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System.Collections;
using System;

public class Enemy : Character {

    [SerializeField]
    protected Player player;

    void Start()
    {
        
    }

    // Method implemented for melee type attacks
    public override void Attack()
    {
        throw new NotImplementedException();
    }

    // Method to check if the player is in range of the current Enemy
    public override bool IsInRange(float range)
    {
        
        if (Vector3.Distance(player.GetComponent<Transform>().position, GetComponent<Transform>().position) < range)
        {
            return true;
        }
        else {
            return false;
        }
    }

    // Common Death check for all enemy types
    public override void CheckDeath()
    {
        if (health <= 0)
        {
            Debug.Log("You have killed an enemy!");
            Destroy(gameObject);
        }
    }

    // Method for projectile attacks
    public override void ProjectileAttack(float speed, Vector3 direction)
    {
        throw new NotImplementedException();
    }
}
