using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Perform a melee hit which damages all enemies caught in the sweep
public class WarriorAction : MonoBehaviour
{
    Warrior warriorScript;

    private Collider swordCollider;
    private Animator swordAnim;

    [HideInInspector]
    public EnemyCharacter enemyHit;

    [HideInInspector]
    public bool currentlyAttacking;

    private bool currentlyOnCooldown;

    private void Start()
    {
        swordCollider = GetComponent<BoxCollider>();
        swordAnim = GetComponent<Animator>();

        warriorScript = GetComponentInParent<Warrior>();

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

        currentlyAttacking = true;

        StartCoroutine("AttackCooldown");

        yield return new WaitForSeconds(0.15f);
        swordCollider.enabled = false;
        currentlyAttacking = false;
    }

    //Stops player from attacking momentarily.
    private IEnumerator AttackCooldown()
    {
        currentlyOnCooldown = true;
        yield return new WaitForSeconds(0.45f);
        currentlyOnCooldown = false;

    }
}
