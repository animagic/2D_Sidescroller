/*
   Turret Data Model.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System.Collections;

public class Turret : Enemy {

    void Awake()
    {
        SetTurretStats();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int GetHealth()
    {
        return health;
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetRange()
    {
        return range;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }
    
    public void SetTurretStats()
    {
        if (health == 0)
        {
            health = 60;
        }

        if (damage == 0)
        {
            damage = 20;
        }

        if (range == 0)
        {
            range = 20f;
        }
        
        if (attackSpeed == 0)
        {
            attackSpeed = 2f;
        }
    }
}
