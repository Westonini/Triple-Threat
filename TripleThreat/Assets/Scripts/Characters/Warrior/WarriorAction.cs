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

    private void Start()
    {
        swordCollider = GetComponent<BoxCollider>();
        swordAnim = GetComponent<Animator>();

        warriorScript = GetComponentInParent<Warrior>();

        swordAnim.keepAnimatorControllerStateOnDisable = true;
    }

    private void OnDisable()
    {
        swordAnim.ResetTrigger("Attack");
        swordCollider.enabled = false;
    }

    private void Update()
    {
        //If the player presses the "Fire1" key, stop then start the WarriorAttack coroutine
        if (Input.GetButtonDown("Fire1"))
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
    public IEnumerator WarriorAttack()
    {
        swordCollider.enabled = true;
        swordAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.15f);
        swordCollider.enabled = false;
    }
}
