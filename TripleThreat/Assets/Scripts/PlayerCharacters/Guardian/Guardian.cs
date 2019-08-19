using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the Guardian gameobject and is main script for the character.
public class Guardian : PlayerCharacter //Inherits from PlayerCharacter
{
    //Guardian Stats:
    //Can't deal damage but can block all damage; takes no additonal damage when hit; slow speed (2.5)


    GuardianAction GA;

    private void Awake()
    {
        //Get the GuardianAction script from the child object.
        GA = GetComponentInChildren<GuardianAction>();
    }

    protected override void Update()
    {
        if (!GA.currentlyCharging)
        {
            MovementAnimationInput();
            Aim();
        }
    }

    protected override void FixedUpdate()
    {
        if (!GA.currentlyCharging)
            Movement();
    }

    //Guardians's walk speed gets changed within this override function.
    protected override void Movement()
    {
        //If the player currently isn't blocking set walkSpeed to default walk speed (slow speed)
        if (!GA.currentlyBashing)
        {
            walkSpeed = defaultWalkSpeed;
        }
        //If the player currently is blocking set walkSpeed to default walk speed - 1 (very slow speed)
        else
        {
            walkSpeed = defaultWalkSpeed - 1;
        }

        base.Movement();
    }

    //Gets called from the GuardianAction script. Deals 0 damage but knocks enemies back.
    public override void DealDamage<T>(T component)
    {
        //Set enemyScript to equal the component passed in as a parameter, taken from the EnemyCharacter script
        EnemyCharacter enemyScript = component as EnemyCharacter;

        //Deal 0 damage but push enemies back.
        //If the guardian isn't currently bashing, set knockback to 375
        if (!enemyScript.isInvincible && !GA.currentlyBashing && !GA.currentlyCharging)
        {
            enemyScript.TakeDamage(0, transform.position, 375);
        }
        //If the guardian is currently bashing, set knockback to 475
        else if (!enemyScript.isInvincible && GA.currentlyBashing)
        {
            enemyScript.TakeDamage(0, transform.position, 475);
        }
        //If the guardian is currently charging, set knockback to 550
        else if (!enemyScript.isInvincible && GA.currentlyCharging)
        {
            enemyScript.TakeDamage(0, transform.position, 550);
        }
    }
}
