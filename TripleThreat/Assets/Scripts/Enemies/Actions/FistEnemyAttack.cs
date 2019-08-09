using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Will attack any player character if within close enough range.
public class FistEnemyAttack : MonoBehaviour
{  
    [HideInInspector] public PlayerCharacter playerHit;

    FistEnemy enemyScript;

    void Start()
    {
        //Get FistEnemy script from parent
        enemyScript = GetComponentInParent<FistEnemy>();
    }

    //If the grunt touches a player save the player's PlayerCharacter script to playerHit so that it can be used within the enemy's script to deal damage.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHit = other.gameObject.GetComponent<PlayerCharacter>();
            enemyScript.DealDamage(playerHit);
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
