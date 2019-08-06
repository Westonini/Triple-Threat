using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Archer archerScript;

    [HideInInspector]
    public EnemyCharacter enemyHit;

    private void Start()
    {
        archerScript = GameObject.Find("Archer").GetComponent<Archer>();
    }

    void OnTriggerEnter(Collider other)
    {
        //If the arrow touches the ground, destroy it
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) //Ground Layer
        {
            Destroy(gameObject);
        }

        //If the arrow touches an enemy, call DealDamage with the enemy's enemyscript passed in as a parameter
        if (other.gameObject.tag == "Enemy") //Enemy Layer
        {
            enemyHit = other.gameObject.GetComponent<EnemyCharacter>();
            archerScript.DealDamage(enemyHit);
            Destroy(gameObject);
        }
    }
}
