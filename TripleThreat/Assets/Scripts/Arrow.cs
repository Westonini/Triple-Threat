using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Acts as a parent class for PlayerArrow & EnemyArrow
public abstract class Arrow : MonoBehaviour
{
    Rigidbody arrowRB;

    [HideInInspector]
    public EnemyCharacter enemyHit;

    protected virtual void Start()
    {
        //Get the arrow's rigidbody
        arrowRB = GetComponent<Rigidbody>();

        //Start the ArrowGravity coroutine as soon as this object is instantiated.
        StartCoroutine("ArrowGravity");
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //If the arrow touches the ground, destroy it
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) //Ground Layer
        {
            Destroy(gameObject);
        }
    }

    //Starts off with the arrow having no gravity and then 0.2 seconds later enables it.
    private IEnumerator ArrowGravity()
    {
        arrowRB.useGravity = false;
        yield return new WaitForSeconds(0.2f);
        arrowRB.useGravity = true;
    }
}
