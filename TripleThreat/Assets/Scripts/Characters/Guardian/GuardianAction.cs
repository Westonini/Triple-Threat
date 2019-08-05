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
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            shieldCollider.enabled = true;
            shieldRenderer.enabled = true;
            shieldAnim.SetTrigger("Bash");
        }
        if (Input.GetButtonUp("Fire1"))
        {
            shieldCollider.enabled = false;
            shieldRenderer.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemyHit = collision.gameObject.GetComponent<EnemyCharacter>();
            guardianScript.DealDamage(enemyHit);
        }
    }
}
