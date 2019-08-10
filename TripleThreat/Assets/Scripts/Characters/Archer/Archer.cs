﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is attached to the Archer gameobject and is main script for the character.
public class Archer : PlayerCharacter //Inherits from PlayerCharacter
{
    //Archer Stats:
    //Long Ranged Great Damage; takes 2 additional damage when hit; below-average speed (4)


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
        if (!AA.currentlyCharging && !AA.currentlyAiming)
        {
            walkSpeed = 4f;
        }
        //If the player currently is drawing an arrow set walkSpeed to 2 (slow speed)
        else if (AA.currentlyCharging)
        {
            walkSpeed = 2f;
        }
        //If the player currently is aiming an arrow set walkSpeed to 1.5 (very slow speed)
        else if (AA.currentlyAiming)
        {
            walkSpeed = 1.5f;
        }

        base.Movement();
    }

    //Archer takes 2 additional damage when hurt.
    public override void TakeDamage(int damageReceived, Vector3 hitFrom)
    {
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
            enemyScript.TakeDamage(3, transform.position, 500);
        }
    }
}
