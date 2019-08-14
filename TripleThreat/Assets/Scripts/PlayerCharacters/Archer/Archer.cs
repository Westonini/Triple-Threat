using System.Collections;
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
        //Get the main module of the dustParticles to change the speed in the following code
        var main = dustParticles.main;

        //If the player currently isn't charging an arrow set walkSpeed to default walk speed (below-average speed)
        if (!AA.currentlyCharging && !AA.currentlyAiming)
        {
            walkSpeed = defaultWalkSpeed;
            main.startSpeed = 7f;
        }
        //If the player currently is drawing an arrow set walkSpeed to default walk speed - 2 (slow speed)
        else if (AA.currentlyCharging)
        {
            walkSpeed = defaultWalkSpeed - 2;
            main.startSpeed = 3.5f;
        }
        //If the player currently is aiming an arrow set walkSpeed to default walk speed - 2.5 (very slow speed)
        else if (AA.currentlyAiming)
        {
            walkSpeed = defaultWalkSpeed - 2.5f;
            main.startSpeed = 2f;
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
