using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the Grunt gameobject and is main script for the character.
public class Grunt : EnemyCharacter //Inherits from EnemyCharacter
{
    //Set health then call parent Start()
    protected override void Start()
    {
        enemyHealth = 3;
        base.Start();
    }

    //Set walkspeed then call parent Movement()
    protected override void Movement()
    {
        walkSpeed = 4;
        base.Movement();
    }

    //Gets called from the GruntAction script. Deals damage to and knocksback the target while also doing a hit sound effect.
    public override void DealDamage<T>(T component)
    {
        //Set playerScript to equal the component passed in as a parameter, taken from the PlayerCharacter script
        PlayerCharacter playerScript = component as PlayerCharacter;

        //Hit Sound Fx

        //Deal damage to player as long as the player isn't currently invincible
        if (!PlayerCharacter.isInvincible)
        {
            playerScript.TakeDamage(1, transform.position);
        }
    }
}
