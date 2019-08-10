using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : Arrow //Inherits from Arrow parent class
{
    RangedEnemy enemyScript;
    PlayerCharacter playerHit;

    protected override void Start()
    {
        //Get the RangedEnemy script from the object that instantiated this object.
        enemyScript = GetComponentInParent<RangedEnemy>();

        //Change this arrow's parent to null so incase the parent object dies, this object doesn't die with it
        gameObject.transform.parent = null;

        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //If the arrow touches the shield, destroy the arrow and subtract one durabiltiy point from the shield.
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield")) //Shield Layer
        {
            GuardianAction.shieldHealth -= 1;
            Destroy(gameObject);
        }

        //If the arrow touches a player, call the enemy's DealDamage() function and pass in the player's PlayerCharacter script.
        else if (other.gameObject.tag == "Player") //Player Tag
        {
            playerHit = other.gameObject.GetComponent<PlayerCharacter>();
            enemyScript.DealDamage(playerHit);
            Destroy(gameObject);
        }

        base.OnTriggerEnter(other);
    }
}
