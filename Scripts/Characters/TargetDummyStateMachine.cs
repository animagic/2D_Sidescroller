/*
    State Machine for the Target Dummy.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TargetDummyStateMachine : TargetDummy {

    Dictionary<Dummystates, Action> fsm = new Dictionary<Dummystates, Action>();

    private enum Dummystates
    {
        IDLE,
        TAKEHIT,

        NUM_STATES
    }

    [SerializeField]
    private float maxColortimer = 1f;

    private float colorTimer;
    private bool targetHit = false;
    private int dummyHealth;

    private Dummystates curState;

    //  -------------------------------------------------------------------------------------------
    //  Unity Standard Functions Start
    //
    #region
    // Use this for initialization
    void Start () {

        GetComponent<Renderer>().material.color = Color.blue;

        fsm.Add(Dummystates.IDLE, StateIdle);
        fsm.Add(Dummystates.TAKEHIT, StateTakeHit);

        SetState(Dummystates.IDLE);
	}
	
	// Update is called once per frame
	void Update () {
        fsm[curState].Invoke();
	}

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.transform.tag == "Projectile")
        {
            targetHit = true;
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

    
    public void TakeHitColorChange()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void NotHitColorChange()
    {
        GetComponent<Renderer>().material.color = Color.blue;
    }

    public void SetTargetHit()
    {
        targetHit = true;
    }

    public void SetTargetNotHit()
    {
        targetHit = false;
    }
    #endregion
    //  
    //  Helper Functions End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  State Functions Start
    //
    #region
    void SetState(Dummystates nextState)
    {
        if (nextState != curState)
        {
            colorTimer = 0;
            curState = nextState;
        }
    }
    void StateIdle()
    {
        if (!targetHit)
        {
            NotHitColorChange();
        }
        else if(targetHit)
        {
            SetState(Dummystates.TAKEHIT);
        }

    }

    void StateTakeHit()
    {
        SetTargetNotHit();
        TakeHitColorChange();
        colorTimer += Time.deltaTime;

        if (colorTimer >= maxColortimer)
        {
            SetState(Dummystates.IDLE);
        }
    }
    #endregion
    //  
    //  State Functions End
    //  -------------------------------------------------------------------------------------------

}
