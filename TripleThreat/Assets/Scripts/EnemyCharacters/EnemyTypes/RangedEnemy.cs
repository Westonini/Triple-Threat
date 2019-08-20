using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the enemy gameobject and is main script for the character.
public class RangedEnemy : EnemyCharacter //Inherits from EnemyCharacter
{
    //Charge time for the ranged attack
    [Space]
    public float castTime;

    //RangedEnemy's attack script
    RangedEnemyAttack attackScript;

    protected override void Start()
    {
        //Get the attack script
        attackScript = GetComponentInChildren<RangedEnemyAttack>();

        //Call base Start()
        base.Start();
    }

    //Adds Aim() to Update
    protected override void Update()
    {
        MovementAnimationInput();
        Aim();
    }

    protected override void Movement()
    {
        //If the player is within range and a wall isn't in the way set the walk speed to 0 to start charging. Otherwise set the walkspeed to whatever it's set at.
        //When the enemy has 0 speed, stop the dust particles.
        if (attackScript.playerWithinRange && !attackScript.wallHit && isTouchingGround)
        {
            agent.isStopped = true;
            //agent.speed = 0;
            animationsScript.ToggleDustParticles(false);
        }
        else if (!isTouchingGround)
        {
            agent.isStopped = false;
            agent.speed = 9;
            animationsScript.ToggleDustParticles(false);
        }
        else
        {
            agent.isStopped = false;
            agent.speed = agentDefaultSpeed;
            animationsScript.ToggleDustParticles(true);
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
