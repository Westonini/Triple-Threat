using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Acts as a parent class for PlayerArrow & EnemyArrow
public abstract class Arrow : MonoBehaviour
{
    protected Rigidbody arrowRB;
    protected Collider arrowCollider;
    protected MeshRenderer arrowRenderer;
    protected ParticleSystem arrowParticles;

    [HideInInspector]
    public EnemyCharacter enemyHit;

    List<string> possibleShootSounds = new List<string>();

    protected virtual void Start()
    {
        //Get the arrow's components
        arrowRB = GetComponent<Rigidbody>();
        arrowCollider = GetComponent<Collider>();
        arrowRenderer = GetComponent<MeshRenderer>();
        arrowParticles = GetComponentInChildren<ParticleSystem>();

        //Start the ArrowGravity coroutine as soon as this object is instantiated.
        StartCoroutine("ArrowGravity");

        //Add shoot sounds to the possibleSounds list.
        possibleShootSounds.Add("BowShoot1"); possibleShootSounds.Add("BowShoot2");

        //Play random shoot sound
        FindObjectOfType<AudioManager>().PlayRandom(possibleShootSounds);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        //If the arrow touches the ground, turn off its collider and renderer.
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground")) //Ground Layer
        {
            DisableArrow();
        }
    }

    //Starts off with the arrow having no gravity and then 0.2 seconds later enables it.
    private IEnumerator ArrowGravity()
    {
        arrowRB.useGravity = false;
        yield return new WaitForSeconds(0.2f);
        arrowRB.useGravity = true;
    }

    protected void DisableArrow()
    {
        //Disable the collider and renderer, freeze the position, and stop additional particles from spawning.
        //This is so that the trail and particles don't instantly disappear when they hit something.
        arrowCollider.enabled = false;
        arrowRenderer.enabled = false;
        arrowRB.constraints = RigidbodyConstraints.FreezePosition;
        arrowParticles.Stop();
    }
}
