using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyAttack : MonoBehaviour
{
    [HideInInspector] public PlayerCharacter playerHit;

    RangedEnemy enemyScript;
    SwapCharacters swapScript;

    float castTime;
    float distanceFromPlayer;
    [HideInInspector] public bool playerWithinRange;

    void Start()
    {
        //Get RangedEnemy script from parent
        enemyScript = GetComponentInParent<RangedEnemy>();

        //Get SwapCharacters script
        swapScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SwapCharacters>();

        //Get castTime that was set in parent script.
        castTime = enemyScript.castTime;
    }

    void Update()
    {
        //Calculate the distance between this character and the player character
        distanceFromPlayer = Vector3.Distance(swapScript.currentCharacter.transform.position, transform.position);

        if (distanceFromPlayer <= 15)
        {
            playerWithinRange = true;
        }
        else
        {
            playerWithinRange = false;
        }

    }
}
