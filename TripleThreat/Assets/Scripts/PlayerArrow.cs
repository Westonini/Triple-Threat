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

        base.Start();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        //If the arrow touches an enemy, call DealDamage with the enemy's enemyscript passed in as a parameter
        if (other.gameObject.tag == "Enemy") //Enemy Tag
        {
            enemyHit = other.gameObject.GetComponent<EnemyCharacter>();
            archerScript.DealDamage(enemyHit);
            Destroy(gameObject);
        }

        base.OnTriggerEnter(other);
    }
}
