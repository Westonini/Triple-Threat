﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponEnemyAttack : MonoBehaviour
{
    [HideInInspector] public PlayerCharacter playerHit;

    public float attackCooldownTime;
    public float distanceAwayFromPlayerToAttack;
    public float attackTime;
    float distanceFromPlayer;
    [HideInInspector] public bool isCurrentlyAttacking;
    private bool playerWithinRange;

    public bool justBrokeShield;

    Collider weaponCollider;
    Animator weaponAnim;
    
    MeleeWeaponEnemy enemyScript;
    SwapCharacters swapScript;

    void Start()
    {
        //Get the weapon's collider and animator
        weaponCollider = GetComponent<Collider>();
        weaponAnim = GetComponent<Animator>();

        //Get MeleeWeaponEnemy script from parent
        enemyScript = GetComponentInParent<MeleeWeaponEnemy>();

        //Get SwapCharacters script
        swapScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SwapCharacters>();

        //Added so that the animations don't bug.
        weaponAnim.keepAnimatorControllerStateOnDisable = true;
    }

    void Update()
    {
        DistanceFromPlayer();

        if (playerWithinRange && !isCurrentlyAttacking)
        {
            StartCoroutine("Attack");
        }
    }

    //If the weapon touched a player, set that players's gameobject to playerHit and  call DealDamage with playerHit as the parameter passed in.
    private void OnTriggerEnter(Collider other)
    {
        //If the shield is hit, disable the weapon collider, break the shield, and increase the repair time.
        if (other.gameObject.layer == LayerMask.NameToLayer("Shield")) //Shield Layer
        {
            weaponCollider.enabled = false;
            justBrokeShield = true;

            GuardianAction.shieldHealth = 0;
            ShieldRepair.shieldRepairTime = 5f;

            //Knockback but deal 0 damage
            playerHit = other.gameObject.GetComponentInParent<PlayerCharacter>();
            enemyScript.DealDamage(playerHit);
        }

        //If the player is hit, disable the weapon collider and pass in the player's script into DealDamage().
        if (other.gameObject.tag == "Player")
        {
            weaponCollider.enabled = false;

            playerHit = other.gameObject.GetComponent<PlayerCharacter>();
            enemyScript.DealDamage(playerHit);
        }
    }

    void DistanceFromPlayer()
    {
        //Calculate the distance between this character and the player character
        distanceFromPlayer = Vector3.Distance(swapScript.currentCharacter.transform.position, transform.position);

        //If this enemy and player are within (maxRangeFromPlayer) units of eachother, set playerWithinRange to true. Otherwise set it false and stop any current drawing.
        if (distanceFromPlayer <= distanceAwayFromPlayerToAttack)
        {
            playerWithinRange = true;
        }
        else
        {
            playerWithinRange = false;
        }
    }

    IEnumerator Attack()
    {
        isCurrentlyAttacking = true;

        weaponCollider.enabled = true;
        weaponAnim.SetTrigger("Attack");

        yield return new WaitForSeconds(attackTime);

        weaponCollider.enabled = false;

        yield return new WaitForSeconds(attackCooldownTime);

        isCurrentlyAttacking = false;

    }
}
