using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the enemy gameobject and is main script for the character.
public class RangedEnemy : EnemyCharacter //Inherits from EnemyCharacter
{
    //Charge time for the ranged attack
    [Space]
    public float castTime;
    //Used to reset the walkSpeed later.
    private float speed;

    //RangedEnemy's attack script
    RangedEnemyAttack attackScript;

    protected override void Start()
    {
        //Get the attack script
        attackScript = GetComponentInChildren<RangedEnemyAttack>();

        //set Speed to walkSpeed;
        speed = walkSpeed;

        //Call base Start()
        base.Start();
    }

    protected override void Movement()
    {
        //If the player is within range set the walk speed to 0 to start charging. Otherwise set the walkspeed to whatever it's set at.
        //When the enemy has 0 speed, stop the dust particles.
        if (attackScript.playerWithinRange)
        {
            walkSpeed = 0;
            dustParticles.Stop();
        }
        else
        {
            walkSpeed = speed;
            if (!dustParticles.isEmitting)
            {
                dustParticles.Play();
            }
        }

        base.Movement();
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
