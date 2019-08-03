using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public static bool playerIsTouchingGround;

    private void OnTriggerEnter(Collider other)
    {
        //If player is touching the ground
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerIsTouchingGround = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //If player isn't touching the ground
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            playerIsTouchingGround = false;
        }
    }
}
