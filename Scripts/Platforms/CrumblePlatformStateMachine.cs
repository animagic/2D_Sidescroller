/*
   Crumbling Platform State Machine.  Does not require a Data Model.  Current iteration
   simply removes the platform from being rendered, but a shrinking or other animation
   would be prefered.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CrumblePlatformStateMachine : MonoBehaviour {

    Dictionary<CrumblePlatformStates, Action> fsm = new Dictionary<CrumblePlatformStates, Action>();

    private enum CrumblePlatformStates
    {
        IDLE,
        CRUMBLED,

        NUM_STATES
    }

    CrumblePlatformStates curState;

    [SerializeField]
    private float maxPlatformTimer;

    private bool platformExists;
    private float platformTimer;

    //only used in the inspector to check the Collider object on the character hitting the platform
    //Decomment the next line to see in the inspector
    //[SerializeField]
    private Collider2D playerCollider;

    //  -------------------------------------------------------------------------------------------
    //  Standard Unity Methods Start
    //
    #region
    // Use this for initialization
    void Start () {
        SetPlatformTimer();
        platformExists = true;

        fsm.Add(CrumblePlatformStates.IDLE, StateIdle);
        fsm.Add(CrumblePlatformStates.CRUMBLED, StateCrumbled);

        SetState(CrumblePlatformStates.IDLE);

    }
	
	// Update is called once per frame
	void Update () {

        fsm[curState].Invoke();
    }

    // Object check to make sure that the platform is accepting only the Player as a trigger
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            playerCollider = col;
        }
        else
        {
            playerCollider = null;
        }
    }

    // Runs the timer and check for the platforms lifecycle.  As long as the player is on the
    // platform, the time is running.  Jumping off the platform resets the timer.
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            platformTimer += Time.deltaTime;
        }
        else
        {
            platformTimer = 0;
        }

        if (platformTimer >= maxPlatformTimer)
        {
            platformExists = false;
        }
        else
        {
            platformExists = true;
        }
    }

    // Resets all variables when the player leaves the Crumble trigger
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            platformTimer = 0;
            col = null;
            playerCollider = null;
        }
    }
    #endregion
    //  
    //  Standard Unity Methods End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  Helper Methods Start
    //
    #region
    // Set default for platform timer if the user does not set one in the inspector
    void SetPlatformTimer()
    {
        if (maxPlatformTimer == 0)
        {
            maxPlatformTimer = 1.5f;
        }
    }
    // Turns the platform off
    void CrumblePlatform()
    {
        GetComponent<Transform>().parent.GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Transform>().parent.GetComponent<BoxCollider2D>().enabled = false;
    }

    // Turns the platform on
    void RenewPlatform()
    {
        GetComponent<Transform>().parent.GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Transform>().parent.GetComponent<BoxCollider2D>().enabled = true;
    }

    #endregion
    //  
    //  Helper Methods End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  State Methods Start
    //
    #region
    void SetState(CrumblePlatformStates nextState)
    {
        curState = nextState;
    }

    void StateIdle()
    {        
        if(!platformExists)
        { 
            platformTimer = 0;
            SetState(CrumblePlatformStates.CRUMBLED);
        }
    }

    void StateCrumbled()
    {
        CrumblePlatform();
        platformTimer += Time.deltaTime;

        if(platformTimer >= maxPlatformTimer)
        {
            RenewPlatform();
            platformExists = true;
            platformTimer = 0;
            SetState(CrumblePlatformStates.IDLE);
        }
    }
    #endregion
    //  
    //  State Functions End
    //  -------------------------------------------------------------------------------------------
}
