/*
    Base Data Model for all Characters in the game, including the Player and all Enemies.
    We Initialize all common variables as protected so that only derivatives of this class
    can access them.  Common methods are initialized here as abstract so that they can be
    morphed in derivative scripts as needed (and so we don't forget to create a method).

    I am not 100% sure of "best" approach to chaining inheritence between scripts, but
    I tried to implement it as much as possible in this Lab.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/
using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour {

    
    
    [SerializeField]
    protected int health;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float range;
    [SerializeField]
    protected float attackSpeed;
    [SerializeField]
    protected bool onCooldown = false;

    // Shared method for all characters to take damage
    public void TakeDamage(int damage)
    {
        health -= damage;
        CheckDeath();
    }

    // Shared method that resets the cooldown on attacks or abilities
    public void OffCooldown()
    {
        onCooldown = false;
    }

    // Handles any death check and animations
    public abstract void CheckDeath();

    // Check to see if an object is in range
    public abstract bool IsInRange(float range);

    // Attack method to be morphed per enemy type
    public abstract void Attack();

    //Projectile Attack method to be morphed per enemy type
    public abstract void ProjectileAttack(float speed, Vector3 direction);

}
