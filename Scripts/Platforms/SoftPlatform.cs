/*
   Moving Platform State Machine.  Does not require a Data Model.

    Author: Wayne Work
    Date: 3/08/2016
    Credit: Reference Material from Full Sail Game Development class (March 2016)
            Metal Texture from http://bgfons.com/download/637
*/

using UnityEngine;
using System.Collections;

public class SoftPlatform : MonoBehaviour {

    void OnTriggerEnter2D (Collider2D col)
    {

        if (col.tag == "Player" )
        {
            Physics2D.IgnoreCollision(col, GetComponent<Transform>().parent.GetComponent<Collider2D>(), true);

        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" )
        {
            Physics2D.IgnoreCollision(col, GetComponent<Transform>().parent.GetComponent<Collider2D>(), false);

        }
    }
}
