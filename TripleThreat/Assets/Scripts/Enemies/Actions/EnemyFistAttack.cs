using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Will attack any player character if within close enough range.
public class EnemyFistAttack : MonoBehaviour
{  
    private Collider hitCollider;

    [HideInInspector] public PlayerCharacter playerHit;

    void Start()
    {
        //Get the collider of the hit area.
        hitCollider = GetComponent<BoxCollider>();
    }

    //If the grunt touches a player save the player's PlayerCharacter script to playerHit so that it can be used within the enemy's script to deal damage.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHit = other.gameObject.GetComponent<PlayerCharacter>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHit = null;
        }
    }
}
