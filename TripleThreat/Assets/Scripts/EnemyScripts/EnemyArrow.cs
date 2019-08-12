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

        //Set the parent to the Instantiated Objects gameobject after getting the enemy script
        gameObject.transform.SetParent(InstantiateParticles.instantiatedObjects.transform);

        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //If the arrow touches the shield, destroy the arrow and subtract one durabiltiy point from the shield.
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield")) //Shield Layer
        {
            GuardianAction.SubtractShieldHealth(1);

            DisableArrow();
        }

        //If the arrow touches a player, call the enemy's DealDamage() function and pass in the player's PlayerCharacter script.
        else if (other.gameObject.tag == "Player") //Player Tag
        {
            playerHit = other.gameObject.GetComponent<PlayerCharacter>();
            enemyScript.DealDamage(playerHit);

            DisableArrow();
        }

        base.OnTriggerEnter(other);
    }
}
