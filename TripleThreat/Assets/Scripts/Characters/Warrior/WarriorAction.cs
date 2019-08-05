using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            StopCoroutine("WarriorAttack");
            StartCoroutine("WarriorAttack");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyHit = other.gameObject.GetComponent<EnemyCharacter>();
            warriorScript.DealDamage(enemyHit);
        }
    }

    public IEnumerator WarriorAttack()
    {
        swordCollider.enabled = true;
        swordAnim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.15f);
        swordCollider.enabled = false;
    }
}
