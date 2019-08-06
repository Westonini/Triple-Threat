using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerCharacter //Inherits from PlayerCharacter
{
    //Archer Stats:
    //Long Ranged Good Damage; takes 2 additional damage when hit; below-average speed (4)


    ArcherAction AA;

    private void Awake()
    {
        //Get the ArcherAction script from the child object.
        AA = GetComponentInChildren<ArcherAction>();
    }

    //Archer's walk speed gets changed within this override function.
    protected override void Movement()
    {
        //If the player currently isn't charging an arrow set walkSpeed to 4 (below-average speed)
        if (!AA.currentlyCharging)
        {
            walkSpeed = 4f;
        }
        //If the player currently is charging an arrow set walkSpeed to 2 (very slow speed)
        else
        {
            walkSpeed = 2f;
        }

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
            enemyScript.TakeDamage(2, transform.position, 550);
        }
    }
}
