using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemy death coroutine.
public class EnemyDeath : CharacterDeath //Inherits from Character death
{
    EnemyCharacter enemyScript;

    void Awake()
    {
        enemyScript = GetComponent<EnemyCharacter>();
        characterRB = GetComponent<Rigidbody>();
        characterAnim = GetComponent<Animator>();
        animationsScript = GetComponent<CharacterMovementAnimations>();
    }

    void OnEnable()
    {
        enemyScript._enemyDied += HandleDeath;
    }

    private void OnDisable()
    {
        enemyScript._enemyDied -= HandleDeath;
    }

    IEnumerator DeathIEnum()
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
        animationsScript.ToggleDustParticles(false);
        enemyScript.enabled = false;

        //Rigidbody constraint changes for a ragdoll-type effect
        characterRB.constraints = RigidbodyConstraints.None;
        characterRB.constraints = RigidbodyConstraints.FreezeRotationY;
        characterRB.mass = 0.01f;

        //Play sound
        FindObjectOfType<AudioManager>().Play("EnemyDeath", transform);

        //If the enemy character is active, instantiate blood particles every so often and end it with a smoke particle.
        if (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(2f);
            InstantiateParticles.InstantiateParticle(transform, enemyScript.bloodParticles, 5f, 2.5f);
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
