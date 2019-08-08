using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody arrowRB;
    Archer archerScript;

    [HideInInspector]
    public EnemyCharacter enemyHit;

    private void Start()
    {
        //Get the Archer script from the Archer gameobject
        archerScript = GameObject.Find("Archer").GetComponent<Archer>();
        //Get the arrow's rigidbody
        arrowRB = GetComponent<Rigidbody>();

        //Start the ArrowGravity coroutine as soon as this object is instantiated.
        StartCoroutine("ArrowGravity");
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

    //Starts off with the arrow having no gravity and then 0.2 seconds later enables it.
    IEnumerator ArrowGravity()
    {
        arrowRB.useGravity = false;
        yield return new WaitForSeconds(0.2f);
        arrowRB.useGravity = true;
    }
}
