using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hold left mouse button to block attacks and push attackers back. The initial mouse click will also push back enemies
public class GuardianAction : MonoBehaviour
{
    Guardian guardianScript;

    private Collider shieldCollider;
    private Animator shieldAnim;
    private MeshRenderer shieldRenderer;

    [HideInInspector] public EnemyCharacter enemyHit;

    [HideInInspector] public bool currentlyBlocking;

    [HideInInspector] public static int shieldHealth = 10;
    private bool shieldHealthInvincibility;
    private bool shieldIsBroken;

    private void Start()
    {
        shieldCollider = GetComponent<BoxCollider>();
        shieldAnim = GetComponent<Animator>();
        shieldRenderer = GetComponent<MeshRenderer>();

        guardianScript = GetComponentInParent<Guardian>();

        shieldAnim.keepAnimatorControllerStateOnDisable = true;
    }

    private void OnDisable()
    {
        shieldCollider.enabled = false;
        shieldRenderer.enabled = false;
        currentlyBlocking = false;

        shieldHealthInvincibility = false;
        StopCoroutine("SubtractShieldHealthCooldown");
    }

    void Update()
    {
        //When the player holds the Fire1 key, enable the shield's renderer and collider.
        //Play shield push animation as well
        if (Input.GetButton("Fire1") && !shieldIsBroken)
        {
            shieldCollider.enabled = true;
            shieldRenderer.enabled = true;
            shieldAnim.SetTrigger("Bash");
            currentlyBlocking = true;
        }
        //When the player releases the Fire1 key, disable the shield's renderer and collider.
        if (Input.GetButtonUp("Fire1"))
        {
            shieldCollider.enabled = false;
            shieldRenderer.enabled = false;
            currentlyBlocking = false;
        }


        //If the shield's hp drops under 0, the shieldIsBroken boolean sets to true and the shield becomes unusable until repaired.
        if (shieldHealth <= 0 && !shieldIsBroken)
        {
            shieldIsBroken = true;

            shieldCollider.enabled = false;
            shieldRenderer.enabled = false;
            currentlyBlocking = false;

            //Play shield break sound
        }
        else
        {
            shieldIsBroken = false;
        }
    }

    //If the shield touches an enemy, pass their gameobject in as a parameter in DealDamage in order to deal knockback
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyHit = collision.gameObject.GetComponent<EnemyCharacter>();
            guardianScript.DealDamage(enemyHit);

            if (!shieldHealthInvincibility)
            {
                shieldHealth -= 1;
                StartCoroutine("SubtractShieldHealthCooldown");
            }
        }
    }

    IEnumerator SubtractShieldHealthCooldown()
    {
        shieldHealthInvincibility = true;
        yield return new WaitForSeconds(0.1f);
        shieldHealthInvincibility = false;
    }
}
