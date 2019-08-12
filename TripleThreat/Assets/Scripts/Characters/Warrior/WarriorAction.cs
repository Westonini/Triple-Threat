using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Perform a melee swing which will damage any enemies caught in the attack.
public class WarriorAction : MonoBehaviour
{
    Warrior warriorScript;

    private Collider swordCollider;
    private Animator swordAnim;
    private TrailRenderer swordTrail;

    [HideInInspector]
    public EnemyCharacter enemyHit;

    [HideInInspector]
    public bool currentlyAttacking;

    private bool currentlyOnCooldown;

    private void Start()
    {
        //Get the sword's collider, animator, and trail.
        swordCollider = GetComponent<Collider>();
        swordAnim = GetComponent<Animator>();
        swordTrail = GetComponentInChildren<TrailRenderer>();

        //Get the Warrior script from the parent object
        warriorScript = GetComponentInParent<Warrior>();

        //Added so that the animations don't bug.
        swordAnim.keepAnimatorControllerStateOnDisable = true;
    }

    //Reset these things when the player swaps out of this character.
    private void OnDisable()
    {
        StopCoroutine("WarriorAttack");
        StopCoroutine("AttackCooldown");

        swordAnim.ResetTrigger("Attack");
        swordCollider.enabled = false;
        currentlyAttacking = false;
        currentlyOnCooldown = false;
        swordTrail.enabled = false;
    }

    private void Update()
    {
        //If the player presses the "Fire1" key, stop then start the WarriorAttack coroutine
        //Can only attack if attack isn't currently on cooldown
        if (Input.GetButtonDown("Fire1") && !currentlyOnCooldown)
        {
            StopCoroutine("WarriorAttack");
            StartCoroutine("WarriorAttack");
        }
    }

    //If the sword touched an enemy, set that enemy's gameobject to enemyHit and  call DealDamage with enemyHit as the parameter passed in.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyHit = other.gameObject.GetComponent<EnemyCharacter>();
            warriorScript.DealDamage(enemyHit);
        }
    }

    //Enable the sword's collider and play the sword swing animation
    private IEnumerator WarriorAttack()
    {
        swordCollider.enabled = true;
        swordAnim.SetTrigger("Attack");
        swordTrail.enabled = true;

        currentlyAttacking = true;

        StartCoroutine("AttackCooldown");

        yield return new WaitForSeconds(0.10f);
        swordCollider.enabled = false;
        currentlyAttacking = false;
        swordTrail.enabled = false;
    }

    //Stops player from attacking momentarily.
    private IEnumerator AttackCooldown()
    {
        currentlyOnCooldown = true;
        yield return new WaitForSeconds(0.325f);
        currentlyOnCooldown = false;

    }
}
