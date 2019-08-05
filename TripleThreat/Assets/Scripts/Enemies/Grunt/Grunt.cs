using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : EnemyCharacter
{
    //Set health them call parent Start()
    protected override void Start()
    {
        enemyHealth = 3;
        base.Start();
    }

    //Set walkspeed then call parent Movement()
    protected override void Movement()
    {
        walkSpeed = 4;
        base.Movement();
    }

    //Set knockbackPower then call parent TakeDamage()
    public override void TakeDamage(int damageReceived, Vector3 hitFrom)
    {
        knockbackPower = 400;
        base.TakeDamage(damageReceived, hitFrom);
    }

    //Gets called from the GruntAction script. Deals damage to and knocksback the target while also doing a hit sound effect.
    public override void DealDamage<T>(T component)
    {
        //Set playerScript to equal the component passed in as a parameter, taken from the PlayerCharacter script
        PlayerCharacter playerScript = component as PlayerCharacter;

        //Hit Sound Fx

        //Deal damage to player as long as the player isn't currently invincible
        if (!playerScript.isInvincible)
        {
            playerScript.TakeDamage(1, transform.position);
        }
    }
}
