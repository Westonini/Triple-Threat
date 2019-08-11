using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the enemy gameobject and is main script for the character.
public class MeleeWeaponEnemy : EnemyCharacter
{
    private float speed;

    [Space]
    public float speedWhileAttacking;


    //FistEnemy's attack script
    MeleeWeaponEnemyAttack attackScript;

    protected override void Start()
    {
        //Get the attack script
        attackScript = GetComponentInChildren<MeleeWeaponEnemyAttack>();

        //set Speed to walkSpeed;
        speed = walkSpeed;

        //Call base Start()
        base.Start();
    }

    protected override void Movement()
    {
        //If the player is within range set the walk speed to 0 to start charging. Otherwise set the walkspeed to whatever it's set at.
        if (attackScript.isCurrentlyAttacking)
        {
            walkSpeed = speedWhileAttacking;
        }
        else
        {
            walkSpeed = speed;
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
        if (!PlayerCharacter.isInvincible && !attackScript.justBrokeShield)
        {
            playerScript.TakeDamage(damage, transform.position);
        }
        //Knock the player back and deal no damage if the shield was just broken in the attack
        else if (!PlayerCharacter.isInvincible && attackScript.justBrokeShield)
        {
            playerScript.TakeDamage(0, transform.position);
            attackScript.justBrokeShield = false;
        }
    }
}
