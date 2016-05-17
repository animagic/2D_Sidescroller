/*
    State Machine for the Player.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerStateMachine : MonoBehaviour {

    Dictionary<PlayerStates, Action> fsm = new Dictionary<PlayerStates, Action>();

    private enum PlayerStates
    {
        IDLE,
        RUN,
        JUMP_START,
        IN_AIR,
        SHOOTING,

        NUM_STATES
    }

    PlayerStates curState;

    private Player thePlayer;
    private Rigidbody2D rb;
    [SerializeField]
    private Rigidbody2D currentProjectile;
    private float currentProjectileSpeed;

    [SerializeField]
    private Vector3 direction;

    [SerializeField]
    private bool canJump = true;
    [SerializeField]
    private bool onGround = false;
    [SerializeField]
    private bool hasDoubleJumped = false;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private float maxJumpBtnHoldTime = .4f;
    [SerializeField]
    private float timeHoldingInput;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask walkableLayer;
    [SerializeField]
    float groundCheckRadius = 1f;
    [SerializeField]
    bool cooldown = false;

    [SerializeField]
    private Collider2D coll;


    //  -------------------------------------------------------------------------------------------
    //  Unity Standard Functions Start
    //
    #region
    // Use this for initialization
    void Start () {

        thePlayer = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        thePlayer.GetDamage();
        currentProjectileSpeed = currentProjectile.GetComponent<ProjectileData>().GetProjectileSpeed();
        direction = Vector3.left;
        GetComponent<Transform>().FindChild("FiringPoint").localPosition = new Vector3(-1f, 0f, 0f);

        fsm.Add(PlayerStates.IDLE, StateIdle);
        fsm.Add(PlayerStates.RUN, StateRun);
        fsm.Add(PlayerStates.JUMP_START, StateJumpStart);
        fsm.Add(PlayerStates.IN_AIR, StateInAir);
        fsm.Add(PlayerStates.SHOOTING, StateShooting);

        FindSpawnPoint();

        SetState(PlayerStates.IDLE);
       
    }
    
    // Update is called once per frame
    void Update () {
        GetPlayerDirection();
        fsm[curState].Invoke();
        
    }

    // Object check to make sure that the platform is accepting only the Player as a trigger
    // Then assign the platform as the Player's parent.  This makes the player move with the platform.
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag == "MovingPlatform")
        {
            coll = col.collider;
            GetComponent<Transform>().parent = col.transform;
        }
        else
        {
            GetComponent<Transform>().parent = null;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.transform.tag == "MovingPlatform")
        {
            
            GetComponent<Transform>().parent = null;
            GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        }
    }
    #endregion
    //  
    //  Unity Standard Functions End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  Helper Functions Start
    //
    #region
    //  Checks the player's direction every time a key is pressed to ensure the player is firing
    //  in the correct direction and that the bullet comes out the correct side
    void GetPlayerDirection()
    {
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.left;
            GetComponent<Transform>().FindChild("FiringPoint").localPosition = new Vector3(-1f, 0f, 0f);
        }

        if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.right;
            GetComponent<Transform>().FindChild("FiringPoint").localPosition = new Vector3(1f, 0f, 0f);
        }
    }

    // Method to shoot
    void FireProjectile()
    {

        if (!cooldown)
        {
            cooldown = true;
            Attack(currentProjectileSpeed, direction);
            Invoke("OffCooldown", thePlayer.GetAttackSpeed());
        }
    }

    void OffCooldown()
    {
        cooldown = false;
    }

    // Method to handle shooting
    void HandleFireWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            SetState(PlayerStates.SHOOTING);
        }
    }

    //  Spawns a projectile from the Resources folder at he player's Firing Point (empty gameobject) and gives it a velocity
    public void Attack(float speed, Vector3 direction)
    {
        GameObject clone = (GameObject)Instantiate(Resources.Load("Projectile"), GetComponent<Transform>().FindChild("FiringPoint").GetComponent<Transform>().position,
            GetComponent<Transform>().FindChild("FiringPoint").GetComponent<Transform>().rotation);
        clone.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(direction * speed);
    }

    //  Checks to see if a SpawnPoint object exists, and if it doesn't spawn the player in the Lower-right side of the map
    void FindSpawnPoint()
    {
        if (spawnPoint == null)
        {
            if (GameObject.Find("SpawnPoint") != null)
            {
                rb.GetComponent<Transform>().position = GameObject.Find("SpawnPoint").GetComponent<Transform>().position;
            }
            else
            {
                rb.GetComponent<Transform>().position = new Vector2(13f, 5f);
            }
        }
        else
        {
            rb.GetComponent<Transform>().position = spawnPoint.position;
        }
    }

    // Checks to see if the player is on a "Walkable" layer
    void CheckForGround()
    {
        onGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, walkableLayer);
    }

    void HandleLandOnGround()
    {
        float direction = Input.GetAxis("Horizontal");

        canJump = true;
        hasDoubleJumped = false;

        if (direction != 0)
        {
            SetState(PlayerStates.RUN);
        }
        else
        {
            SetState(PlayerStates.IDLE);
        }
    }

    void HandleHorizontalInput()
    {
        float direction = Input.GetAxis("Horizontal");
        float magnitude = Mathf.Abs(rb.velocity.x);

        

        Vector2 accel;
            
        if(curState == PlayerStates.IN_AIR)
        {
            accel = new Vector2(thePlayer.GetInAirAccel() * direction, 0);
        }
        else
        {
            accel = new Vector2(thePlayer.GetRunAccel() * direction, 0);
        }
           

        if (direction != 0f)
        {
            rb.velocity += accel;
        }

        else if (magnitude <= 0.01f)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (magnitude >= thePlayer.GetMaxRunSpeed())
        {
            rb.velocity = new Vector2(thePlayer.GetMaxRunSpeed() * direction * Time.deltaTime, rb.velocity.y);
        }
    }

    void HandleNotOnGround()
    {
        CheckForGround();

        if (!onGround)
        {
            SetState(PlayerStates.IN_AIR);
        }
    }

    // Used during the character's jump.  Sets the character spinning for a nifty jump effect.
    void StartCharacterSpin()
    {
        float direction = Input.GetAxis("Horizontal");

        Vector3 runningRotate = new Vector3(0, 0, thePlayer.GetRunningRotate() * direction * -1);

        rb.GetComponent<Transform>().Rotate(runningRotate);
    }

    // Quaternion.identity is used to revert the character back to a rotation of 0.
    void StopCharacterSpin()
    {
        rb.GetComponent<Transform>().rotation = Quaternion.identity;
    }
    #endregion
    //  
    //  Helper Functions End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  State Functions Start
    //
    #region
    void SetState(PlayerStates nextState)
    {
        if (nextState != curState)
        {
            
            timeHoldingInput = 0.0f;
            curState = nextState;
            Debug.Log(curState);
        }
    }

    void StateIdle()
    {
        HandleNotOnGround();
        HandleFireWeapon();

            if (Input.GetAxis("Horizontal") != 0f)
        {
            SetState(PlayerStates.RUN);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SetState(PlayerStates.JUMP_START);
        }
    }

    void StateRun()
    {
        HandleHorizontalInput();
        HandleFireWeapon();

        float direction = Input.GetAxis("Horizontal");

        HandleNotOnGround();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetState(PlayerStates.JUMP_START);
        }

        else if (direction == 0f)
        {
            SetState(PlayerStates.IDLE);
        }
   }

    void StateJumpStart()
    {
        rb.velocity = new Vector2(rb.velocity.x, thePlayer.GetJumpStart());
        
        SetState(PlayerStates.IN_AIR);
    }

    void StateInAir()
    {

        HandleHorizontalInput();
        HandleFireWeapon();

        // Checks to see if the player can jump.
        if (canJump)
        {
            if (timeHoldingInput < maxJumpBtnHoldTime && Input.GetKey(KeyCode.Space))
            {
                StartCharacterSpin();
                timeHoldingInput += Time.deltaTime;
                rb.velocity += new Vector2(0, thePlayer.GetJumpAccel());
            }
            else
            {
                canJump = false;
            }
        }
        // checks to see if the player has double jumped but has already jumped once
        else if (!hasDoubleJumped && !canJump  && Input.GetKey(KeyCode.Space))
        {
            canJump = true;
            hasDoubleJumped = true;
            SetState(PlayerStates.JUMP_START);   
        }
        else
        { 
            StopCharacterSpin();
            CheckForGround();

            if(onGround)
            {
                
                HandleLandOnGround();
            }
        }
    }

    void StateShooting()
    {
        FireProjectile();
        CheckForGround();

        if (onGround)
        {
            HandleLandOnGround();
        }
        else
        {
            HandleNotOnGround();
        }
    }
    #endregion
    //  
    //  State Functions End
    //  -------------------------------------------------------------------------------------------

}
