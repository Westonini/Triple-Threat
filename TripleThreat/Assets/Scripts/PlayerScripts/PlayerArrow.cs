using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : Arrow //Inherits from Arrow parent class
{
    Archer archerScript;

    protected override void Start()
    {
        //Get the Archer script from the Archer gameobject
        archerScript = GameObject.Find("Archer").GetComponent<Archer>();

        //Set the parent to the Instantiated Objects gameobject after getting the archer script
        gameObject.transform.SetParent(InstantiateParticles.instantiatedObjects.transform);

        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //If the arrow touches an enemy, call DealDamage with the enemy's enemyscript passed in as a parameter
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy")) //Enemy Layer
        {
            enemyHit = other.gameObject.GetComponent<EnemyCharacter>();
            archerScript.DealDamage(enemyHit);

            DisableArrow();
        }

        base.OnTriggerEnter(other);
    }
}
