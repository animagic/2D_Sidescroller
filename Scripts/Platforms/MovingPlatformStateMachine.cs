/*
   Moving Platform State Machine.  Does not require a Data Model.

    Author: Wayne Work
    Date: 3/10/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MovingPlatformStateMachine : MonoBehaviour {

    Dictionary<MovingPlatformStates, Action> fsm = new Dictionary<MovingPlatformStates, Action>();

    private enum MovingPlatformStates
    {
        IDLE,
        MOVING,

        NUM_STATES
    }

    MovingPlatformStates curState;

    // Timer variables for state transitions
    [SerializeField]
    private float platformPauseTimer;
    [SerializeField]
    private float platformTimer;

    // Platform and endpoint variables for movement
    [SerializeField]
    private Transform platform;
    [SerializeField]
    private Transform bottomAndLeftExtent;
    [SerializeField]
    private Transform topAndRightExtent;

    // Speed that the platform moves, Direction it moves in, and the endpoint it is going to
    [SerializeField]
    private float speed = 2f;
    private Vector2 direction;
    private Transform destination;

    // Variables to allow the user to change the distance that the endpoints are from the platform's placement center
    [SerializeField]
    private float bottomAndLeftExtentVector;
    [SerializeField]
    private float topAndRightExtentVector;

    // Enum values for a dropdown box in the editor to switch between H/V
    [SerializeField]
    private enum movementDirection
    {
        Horizontal,
        Vertical
    }

    // Creates the dropdown box in the editor for the H/V values
    [SerializeField]
    private movementDirection currentMovementDirection;

    //only used in the inspector to check the Collider object on the character hitting the platform
    //Decomment the next line to see in the inspector
    [SerializeField]
    private Collider2D playerCollider;

    //  -------------------------------------------------------------------------------------------
    //  Standard Unity Methods Start
    //
    #region
    // Use this for initialization
    void Start () {

        platformPauseTimer = 2f;
        speed = 8f;

        SetExtentLocations();
        SetEndPoint(bottomAndLeftExtent);

        fsm.Add(MovingPlatformStates.IDLE, StateIdle);
        fsm.Add(MovingPlatformStates.MOVING, StateMoving);

        SetState(MovingPlatformStates.MOVING);
    }
	
	// Update is called once per frame
	void Update () {

        fsm[curState].Invoke();
    }



    #endregion
    //  
    //  Standard Unity Methods End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  Helper Methods Start
    //
    #region
    // Takes an endPoint variable to tell the platform which direction to go
    void SetEndPoint(Transform endPoint)
    {
        destination = endPoint;
        direction = (destination.position - platform.GetComponent<Transform>().position);
    }

    // Moves the platform to the currently selected endPoint
    void MoveToEndPoint()
    {
        platform.GetComponent<Transform>().position = Vector2.MoveTowards(platform.GetComponent<Transform>().position, destination.GetComponent<Transform>().position, speed * Time.deltaTime);
    }

    // Sets default Extent locations for the platform's endPoints if the player does not set values in the extentVector variables in the inspector
    // Another feature to add is to allow the user to drag the extents themselves around wherever they want and use those localPositions as the endPoints
    void SetExtentLocations()
    {
        if (currentMovementDirection == movementDirection.Horizontal)
        {
            if (bottomAndLeftExtentVector == 0)
            {
                bottomAndLeftExtentVector = 1.5f;
            }

            if (topAndRightExtentVector == 0)
            {
                topAndRightExtentVector = 1.5f;
            }

            GetComponent<Transform>().FindChild("bottomExtent").GetComponent<Transform>().localPosition = new Vector2((bottomAndLeftExtentVector - 2 * bottomAndLeftExtentVector), 0);
            GetComponent<Transform>().FindChild("topExtent").GetComponent<Transform>().localPosition = new Vector2(Mathf.Abs(topAndRightExtentVector), 0);
        }
        else if (currentMovementDirection == movementDirection.Vertical)
        {
            if (bottomAndLeftExtentVector == 0)
            {
                bottomAndLeftExtentVector = 6.0f;
            }

            if (topAndRightExtentVector == 0)
            {
                topAndRightExtentVector = 6.0f;
            }

            GetComponent<Transform>().FindChild("bottomExtent").GetComponent<Transform>().localPosition = new Vector2(0, (bottomAndLeftExtentVector - 2 * bottomAndLeftExtentVector));
            GetComponent<Transform>().FindChild("topExtent").GetComponent<Transform>().localPosition = new Vector2(0, Mathf.Abs(topAndRightExtentVector));
        }
    }

    #endregion
    //  
    //  Helper Methods End
    //  -------------------------------------------------------------------------------------------

    //  -------------------------------------------------------------------------------------------
    //  State Methods Start
    //
    #region
    void SetState(MovingPlatformStates nextState)
    {
        curState = nextState;
    }

    void StateIdle()
    {
        platformTimer += Time.deltaTime;

        if (platformTimer >= platformPauseTimer)
        {
            platformTimer = 0;
            SetState(MovingPlatformStates.MOVING);
        }
    }

    /* 
        Moves the platform and checks the distance between the platform's current position and the destination.  Uses
        deltaTime since it is generally a small number and adds a tiny bit of variation to the exact time of the 
        endPoint switch.
    */
    void StateMoving()
    {
        MoveToEndPoint();

        if (Vector2.Distance (platform.GetComponent<Transform>().position, destination.position) < Time.deltaTime)
        {
            if (destination == bottomAndLeftExtent)
            {
                SetEndPoint(topAndRightExtent);
            }
            else
            {
                SetEndPoint(bottomAndLeftExtent);
            }
            SetState(MovingPlatformStates.IDLE);
        }
    }
    #endregion
    //  
    //  State Functions End
    //  -------------------------------------------------------------------------------------------
}
