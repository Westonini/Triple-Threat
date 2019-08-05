using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : PlayerCharacter //Inherits from PlayerCharacter
{
    //Guardian Stats:
    //Can't deal damage but can block all damage; takes no additonal damage when hit; really slow speed (2.5)


    //Guardians's walk speed is set to 2.5 (very slow speed)
    protected override void Movement()
    {
        walkSpeed = 2.5f;
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

        //Shield block Sound Fx

        //Deal damage to enemy as long as the enemy isn't currently invincible
        if (!enemyScript.isInvincible)
        {
            enemyScript.TakeDamage(0, transform.position, 450);
        }
    }
}
