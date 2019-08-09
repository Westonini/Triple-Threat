using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the enemy gameobject and is main script for the character.
public class RangedEnemy : EnemyCharacter //Inherits from EnemyCharacter
{
    //Values that each RangedEnemy variant differs in.
    public int health;
    public float speed;
    public int damage;
    public float castTime;
    [Range(0, 100)] public int knockbackResistPercentage;

    //RangedEnemy's attack script
    RangedEnemyAttack attackScript;

    protected override void Start()
    {
        //Get the attack script
        attackScript = GetComponentInChildren<RangedEnemyAttack>();

        //Sets the enemy's health, speed, and knockback resistance
        enemyHealth = health;
        walkSpeed = speed;
        knockbackResistance = knockbackResistPercentage;

        //Call base Start()
        base.Start();
    }

    protected override void Movement()
    {
        if (attackScript.playerWithinRange)
        {
            walkSpeed = 0;
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
        if (!PlayerCharacter.isInvincible)
        {
            playerScript.TakeDamage(damage, transform.position);
        }
    }
}
