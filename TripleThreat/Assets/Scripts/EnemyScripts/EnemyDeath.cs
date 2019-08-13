using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    Rigidbody characterRB;
    EnemyCharacter enemyScript;
    Animator characterAnim;

    bool currentlyDying;

    public GameObject bloodParticles;
    public GameObject smokeParticles;

    void Awake()
    {
        enemyScript = GetComponent<EnemyCharacter>();
        characterRB = GetComponent<Rigidbody>();
        characterAnim = GetComponent<Animator>();
    }

    void Update()
    {
        //If the enemy's health is <= 0 the Start the death coroutine
        if (enemyScript.health <= 0 && !currentlyDying)
        {
            currentlyDying = true;
            StartCoroutine("EnemyDeathIEnum");
        }
    }

    IEnumerator EnemyDeathIEnum()
    {
        //Set all gameobjects tagged "Equipment" to inactive.
        foreach (Transform equipment in transform) if (equipment.CompareTag("Equipment"))
            {
                equipment.gameObject.SetActive(false);
            }

        //Set the gameobject's layer to EnemyCorpse so that other enemies can't collide with it
        gameObject.layer = LayerMask.NameToLayer("EnemyCorpse");

        //Enable/disable things
        characterAnim.enabled = false;
        enemyScript.dustParticles.Stop();
        enemyScript.enabled = false;

        //Rigidbody constraint changes for a ragdoll-type effect
        characterRB.constraints = RigidbodyConstraints.None;
        characterRB.constraints = RigidbodyConstraints.FreezeRotationY;
        characterRB.mass = 0.1f;

        //If the enemy character is active, instantiate blood particles every so often and end it with a smoke particle.
        if (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(2f);
            InstantiateParticles.InstantiateParticle(transform, bloodParticles, 5f, 2.5f);
            yield return new WaitForSeconds(2f);
        }
        //If the enemy character was inactive due to them falling off the map or some othe reason, only wait for 1.7 seconds
        else
        {
            yield return new WaitForSeconds(1.7f);
        }

        //Instantiate a smoke cloud and destroy the enemy gameobject.
        InstantiateParticles.InstantiateParticle(transform, smokeParticles, 2f, 0.25f);
        Destroy(gameObject);
    }
}
