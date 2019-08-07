using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Guardians's walk speed gets changed within this override function.
    protected override void Movement()
    {
        //If the player currently isn't blocking set walkSpeed to 2.5 (slow speed)
        if (!GA.currentlyBlocking)
        {
            walkSpeed = 2.5f;
        }
        //If the player currently is blocking set walkSpeed to 1.5 (very slow speed)
        else
        {
            walkSpeed = 1.5f;
        }

        base.Movement();
    }

    //Guardian takes 0 additional damage when hurt.
    //Guardian's knockback power is 200.
    public override void TakeDamage(int damageReceived, Vector3 hitFrom)
    {
        knockbackPower = 750;
        base.TakeDamage(damageReceived, hitFrom);
    }

    //Gets called from the GuardianAction script. Deals 0 damage but knocks enemies back.
    public override void DealDamage<T>(T component)
    {
        //Set enemyScript to equal the component passed in as a parameter, taken from the EnemyCharacter script
        EnemyCharacter enemyScript = component as EnemyCharacter;

        //Shield block sound effect

        //Deal 0 damage but push enemies back.
        //If the guardian is currently bashing, set knockback to 475
        if (!enemyScript.isInvincible && GA.currentlyBlocking)
        {
            enemyScript.TakeDamage(0, transform.position, 475);
        }
        //If the guardian is currently bashing, set knockback to 400
        else if (!enemyScript.isInvincible && !GA.currentlyBlocking)
        {
            enemyScript.TakeDamage(0, transform.position, 350);
        }
    }
}
