using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //If the player currently isn't attacking set walkSpeed to 5 (average speed)
        if (!WA.currentlyAttacking)
        {
            walkSpeed = 5f;
        }
        //If the player currently is attacking set walkSpeed to 3 (below-average speed)
        else
        {
            walkSpeed = 3f;
        }
        base.Movement();
    }

    //Warrior takes 1 additional damage when hurt.
    //Warrior's knockback power is 300.
    public override void TakeDamage(int damageReceived, Vector3 hitFrom)
    {
        knockbackPower = 1000;
        base.TakeDamage(damageReceived + 1, hitFrom);
    }

    //Gets called from the WarriorAction script. Deals damage to and knocksback the target while also doing a sword sound effect.
    public override void DealDamage<T>(T component)
    {
        //Set enemyScript to equal the component passed in as a parameter, taken from the EnemyCharacter script
        EnemyCharacter enemyScript = component as EnemyCharacter;

        //Sword Sound Fx

        //Deal damage to enemy as long as the enemy isn't currently invincible
        if (!enemyScript.isInvincible)
        {
            enemyScript.TakeDamage(2, transform.position, 300);
        }
    }
}
