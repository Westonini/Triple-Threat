using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public static bool playerIsTouchingGround;

    [HideInInspector]
    public bool enemyIsTouchingGround;

    private void Start()
    {
        //Reset playerIsTouchingGround to false at every Start of the scene.
        playerIsTouchingGround = false;
    }

    private void OnTriggerEnter(Collider other)
    {      
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //If this script is attached to a player's ground check object and they're touching the ground set bool to true
            if (gameObject.tag == "PlayerGC")
            {
                playerIsTouchingGround = true;
            }
            //If this script is attached to a enemy's ground check object and they're touching the ground set bool to true
            if (gameObject.tag == "EnemyGC")
            {
                enemyIsTouchingGround = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //If this script is attached to a player's ground check object and they're not touching the ground set bool to false
            if (gameObject.tag == "PlayerGC")
            {
                playerIsTouchingGround = false;
            }
            //If this script is attached to a enemy's ground check object and they're not touching the ground set bool to false
            if (gameObject.tag == "EnemyGC")
            {
                enemyIsTouchingGround = false;
            }
        }
    }
}
