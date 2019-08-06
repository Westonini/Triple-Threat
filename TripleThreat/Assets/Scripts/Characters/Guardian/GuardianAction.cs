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

    [HideInInspector]
    public EnemyCharacter enemyHit;

    [HideInInspector]
    public bool currentlyBlocking;

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
    }

    void Update()
    {
        //When the player holds the Fire1 key, enable the shield's renderer and collider.
        //Play shield push animation as well
        if (Input.GetButton("Fire1"))
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
    }

    //If the shield touches an enemy, pass their gameobject in as a parameter in DealDamage in order to deal knockback
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyHit = collision.gameObject.GetComponent<EnemyCharacter>();
            guardianScript.DealDamage(enemyHit);
        }
    }
}
