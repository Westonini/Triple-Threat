using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerCharacter //Inherits from PlayerCharacter
{
    //Archer Stats:
    //Long Ranged Decent Damage; takes 2 additional damage when hit; slow speed (4)


    //Archer's walk speed is set to 4 (slow speed)
    protected override void Movement()
    {
        walkSpeed = 4;
        base.Movement();
    }

    //Archer takes 2 additional damage when hurt.
    //Archer's knockback power is 350.
    public override void TakeDamage(int damageReceived, Vector3 hitFrom)
    {
        knockbackPower = 1150;
        base.TakeDamage(damageReceived + 2, hitFrom);
    }

    //Gets called from the ArcherAction script. The arrow deals damage and knocks back the enemy when it connects
    public override void DealDamage<T>(T component)
    {
        //Set enemyScript to equal the component passed in as a parameter, taken from the EnemyCharacter script
        EnemyCharacter enemyScript = component as EnemyCharacter;

        //Arrow hit Sound Fx

        //Deal damage to enemy as long as the enemy isn't currently invincible
        if (!enemyScript.isInvincible)
        {
            enemyScript.TakeDamage(2, transform.position, 500);
        }
    }
}
