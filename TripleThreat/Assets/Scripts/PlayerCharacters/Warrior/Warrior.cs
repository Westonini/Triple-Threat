using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the Warrior gameobject and is main script for the character.
public class Warrior : PlayerCharacter //Inherits from PlayerCharacter
{
    //Warrior Stats:
    //Close Ranged Good Damage; takes 1 additional damage when hit; average speed (5)


    WarriorAction WA;

    private void Awake()
    {
        //Get the WarriorAction script from the child object.
        WA = GetComponentInChildren<WarriorAction>();
    }

    //Warrior's walk speed gets changed within this override function.
    protected override void Movement()
    {
        //If the player currently isn't acctacking set walkSpeed to the default speed
        if (!WA.currentlyAttacking)
        {
            walkSpeed = defaultWalkSpeed;
        }
        //If the player currently is attacking set walkSpeed to the default speed - 2
        else 
        {
            walkSpeed = defaultWalkSpeed - 2;
        }
        base.Movement();
    }

    //Warrior takes 1 additional damage when hurt.
    public override void TakeDamage(int damageReceived, Vector3 hitFrom)
    {
        base.TakeDamage(damageReceived + 1, hitFrom);
    }

    //Gets called from the WarriorAction script. Deals damage to and knocksback the target while also doing a sword sound effect.
    public override void DealDamage<T>(T component)
    {
        //Set enemyScript to equal the component passed in as a parameter, taken from the EnemyCharacter script
        EnemyCharacter enemyScript = component as EnemyCharacter;

        //Deal damage to enemy as long as the enemy isn't currently invincible
        if (!enemyScript.isInvincible)
        {
            //If attack1 was used, which is the weaker quick attack..
            if (WA.attack1Used)
            {
                enemyScript.TakeDamage(2, transform.position, 300);
                WA.attack1Used = false;
            }
            //If attack2 was used, which is the stronger sweep attack..
            if (WA.attack2Used)
            {
                enemyScript.TakeDamage(3, transform.position, 350);
                WA.attack2Used = false;
            }
        }
    }
}
