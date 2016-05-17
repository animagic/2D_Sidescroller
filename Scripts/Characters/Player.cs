/*
    Data Model for the player.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class Player : Character {

	[SerializeField]
	private float maxRunSpeed = 3.0f;
	[SerializeField]
	private float runAccel = .5f;
	[SerializeField]
	private float inAirAccel = .1f;
	[SerializeField]
	private float jumpAccel = 0.4f;
	[SerializeField]
	private float initJumpSpeed = 12.0f;
	[SerializeField]
	private float runningRotate = 15f;

    [SerializeField]
    private Slider healthBar;

    [SerializeField]
    private Text healthText;

	private Transform enemy;
	private Player player;
    
    //
	//  End Getters
	// 

	void Awake ()
	{
		player = this;
	}
	// Use this for initialization
	void Start () {
        SetPlayerStats();
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePlayerStats();   
	}

	//  -------------------------------------------------------------
	//  Start Helper Methods
	//
	#region
    // Method overrides not used at the moment.  I placed these features inside the State Machine instead.
	public override void Attack()
	{
        throw new NotImplementedException();
    }

    public override void ProjectileAttack(float speed, Vector3 direction)
    {
        throw new NotImplementedException();
    }

    public override bool IsInRange(float range)
	{
		throw new NotImplementedException();
	}

	public override void CheckDeath()
	{
		if (health <= 0)
		{
			Debug.Log("You have died.  Better luck next time!");
			health = 100;
			player.GetComponent<Rigidbody2D>().GetComponent<Transform>().position = GameObject.Find("SpawnPoint").GetComponent<Transform>().position;
		}
	}

    private void SetPlayerStats()
    {
        health = 100;
        healthBar.value = 100;
        healthText.text = health.ToString();
        attackSpeed = .5f;
    }

    // Method to update the health bar at the top of the screen
    private void UpdatePlayerStats()
    {
        healthBar.value = GetHealth();
        healthText.text = health.ToString();
    }

    // Method to allow other scripts to set the player's health as needed.
    public void SetPlayerHealth(int newHealth)
    {
        health = newHealth;
    }
	#endregion
	//
	//  End Helper Methods
	// 

	//  -------------------------------------------------------------
	//  Start Getters
	//
	#region
	public float GetMaxRunSpeed()
	{
		return maxRunSpeed;
	}

	public float GetRunAccel()
	{
		return runAccel;
	}

	public float GetInAirAccel()
	{
		return inAirAccel;
	}

	public float GetJumpAccel()
	{
		return jumpAccel;
	}

	public float GetJumpStart()
	{
		return initJumpSpeed;
	}

	public float GetRunningRotate()
	{
		return runningRotate;
	}

	public int GetHealth()
	{
		return health;
	}

	public Player GetPlayer()
	{
		return player;
	}

    public int GetDamage()
    {
        return damage;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public bool GetOnCooldown()
    {
        return onCooldown;
    }
	#endregion 
	//
	//  End Getters
	//  

	


}
