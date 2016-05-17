/*
   Turret State Machine.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TurretStateMachine : Turret {

    Dictionary<TurretStates, Action> fsm = new Dictionary<TurretStates, Action>();

    private enum TurretStates
    {
        IDLE,
        FIRE,

        NUM_STATES
    }

    [SerializeField]
    private Rigidbody2D currentProjectile;

    private Vector3 direction;
    private float currentProjectileSpeed;
    private float attackTimer;

    // enum and variable used to allow the user to change the turret's firing direction in the inspector.
    [SerializeField]
    private enum TurretDirection
    {
        LEFT,
        RIGHT
    }

    [SerializeField]
    private TurretDirection turrDir;

    TurretStates curState;

    //  -------------------------------------------------------------------------------------------
    //  Unity Standard Functions Start
    //
    #region
    // Use this for initialization

    void Start () {

        SetTurretDirection();
        SetTurretStats();
        player = GameObject.Find("Player").GetComponent<Player>();
        currentProjectileSpeed = currentProjectile.GetComponent<ProjectileData>().GetProjectileSpeed();

        fsm.Add(TurretStates.IDLE, StateIdle);
        fsm.Add(TurretStates.FIRE, StateFire);

        SetState(TurretStates.FIRE);
	}
	
	// Update is called once per frame
	void Update () {
        fsm[curState].Invoke();
	}
    #endregion
    //  
    //  Unity Standard Functions End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  Helper Functions Start
    //
    #region
    // Sets the turret's FiringPoint location and firing direction based on the choice of the user in the inspector.
    void SetTurretDirection()
    {
        if (turrDir == TurretDirection.LEFT)
        {
            direction = Vector3.left;
            GetComponent<Transform>().FindChild("FiringPoint").GetComponent<Transform>().localPosition = new Vector3(-1f, .8f, 0);
        }

        if (turrDir == TurretDirection.RIGHT)
        {
            direction = Vector3.right;
            GetComponent<Transform>().FindChild("FiringPoint").GetComponent<Transform>().localPosition = new Vector3(1.8f, .8f, 0);
        }
    }

    void FireProjectile()
    {
        Attack(currentProjectileSpeed, direction);
    }

    public void Attack(float speed, Vector3 direction)
    {
        GameObject clone = (GameObject)Instantiate(Resources.Load("Projectile"), GetComponent<Transform>().FindChild("FiringPoint").GetComponent<Transform>().position, 
            GetComponent<Transform>().FindChild("FiringPoint").GetComponent<Transform>().rotation);
        clone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(direction * currentProjectileSpeed);
    }
    #endregion
    //  
    //  Helper Functions End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  State Functions Start
    //
    #region
    void SetState(TurretStates nextState)
    {
        if (nextState != curState)
        {
            curState = nextState; 
        }
    }

    void StateIdle()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= GetAttackSpeed())
        {
            SetState(TurretStates.FIRE);
        }
    }

    void StateFire()
    {
        if (IsInRange(GetRange()))
        {
            FireProjectile();
            attackTimer = 0f;
            SetState(TurretStates.IDLE);
        }
    }

    #endregion
    //  
    //  State Functions End
    //  -------------------------------------------------------------------------------------------
}
