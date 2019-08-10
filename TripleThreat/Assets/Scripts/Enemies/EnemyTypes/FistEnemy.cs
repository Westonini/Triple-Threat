using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the enemy gameobject and is main script for the character.
public class FistEnemy : EnemyCharacter //Inherits from EnemyCharacter
{
    //FistEnemy's attack script
    FistEnemyAttack attackScript;

    protected override void Start()
    {
        //Get the attack script
        attackScript = GetComponentInChildren<FistEnemyAttack>();

        //Call base Start()
        base.Start();
    }

    //Deals damage to and knocksback the target while also doing a hit sound effect.
    public override void DealDamage<T>(T component)
    {
        //Set playerScript to equal the component passed in as a parameter, taken from the PlayerCharacter script
        PlayerCharacter playerScript = component as PlayerCharacter;

        //Hit Sound Fx

        //Deal damage to player as long as the player isn't currently invincible
        if (!PlayerCharacter.isInvincible)
        {
            playerScript.TakeDamage(damage, transform.position);
        }
    }
}
