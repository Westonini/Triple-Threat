using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hold left mouse button to block attacks and push attackers back. 
public class GuardianAction : MonoBehaviour
{
    Guardian guardianScript;

    private Collider shieldCollider;
    private Animator shieldAnim;
    private MeshRenderer shieldRenderer;
    private ParticleSystem hitParticles;

    [HideInInspector] public EnemyCharacter enemyHit;

    [HideInInspector] public bool currentlyBlocking;

    public static int shieldHealth = 10;
    public static bool shieldIsHit;
    public static bool shieldHealthInvincibility;

    private bool shieldIsBroken;

    protected delegate void Shield();
    protected event Shield ShieldFunctions;

    private void Start()
    {
        //Get components
        shieldCollider = GetComponent<Collider>();
        shieldAnim = GetComponent<Animator>();
        shieldRenderer = GetComponent<MeshRenderer>();
        hitParticles = GetComponentInChildren<ParticleSystem>();

        guardianScript = GetComponentInParent<Guardian>();

        //Set static booleans to false on the start of the scene
        shieldIsHit = false;
        shieldHealthInvincibility = false;

        //This is to make sure some animations don't bug out.
        shieldAnim.keepAnimatorControllerStateOnDisable = true;

        //Add functions to the ShieldFunctions event.
        ShieldFunctions += ShieldBash;
        ShieldFunctions += ShieldBreak;
        ShieldFunctions += ShieldHit;
    }

    //Set certain things to false when the character is swapped.
    private void OnDisable()
    {
        currentlyBlocking = false;

        shieldHealthInvincibility = false;
        shieldIsHit = false;
        StopCoroutine("SubtractShieldHealthCooldown");
    }

    void Update()
    {
        //Call all the functions within the ShieldFunctions event every frame.
        if (ShieldFunctions != null)
        {
            ShieldFunctions();
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
                SubtractShieldHealth(1);
            }
        }
    }

    //A public static void that can subtract the shield health by any amount from any script
    public static void SubtractShieldHealth(int amount)
    {
        if (!shieldHealthInvincibility)
        {
            shieldHealth -= amount;
            shieldIsHit = true;
        }
    }

    //Do a shield bash if the player presses the Fire1 button
    void ShieldBash()
    {
        //When the player holds the Fire1 key play the shield bash animation and set the currentlyblocking boolean to true
        if (Input.GetButton("Fire1") && !shieldIsBroken)
        {
            shieldAnim.SetTrigger("Bash");
            currentlyBlocking = true;
        }
        //When the player releases the Fire1 key stop the shield bash animation and set the currentlyblocking boolean to false
        if (Input.GetButtonUp("Fire1"))
        {
            shieldAnim.ResetTrigger("Bash");
            currentlyBlocking = false;
        }
    }

    void ShieldBreak()
    {
        //If the shield's hp drops under 0, the shieldIsBroken boolean sets to true and the shield becomes unusable until repaired.
        if (shieldHealth <= 0 && !shieldIsBroken)
        {
            shieldIsBroken = true;

            shieldAnim.ResetTrigger("Bash");
            shieldCollider.enabled = false;
            shieldRenderer.enabled = false;
            currentlyBlocking = false;

            //Make sure the lowest the health can go is 0.
            shieldHealth = 0;

            //Play shield break sound
        }
        else if (shieldHealth > 0 && shieldIsBroken)
        {
            shieldIsBroken = false;
            shieldCollider.enabled = true;
            shieldRenderer.enabled = true;
        }
    }

    //When shield is hit play an impact particle, play a sound, and start the shield invincibility function
    void ShieldHit()
    {      
        if (shieldIsHit)
        {
            //Hit particle
            if (!hitParticles.isEmitting)
            {
                hitParticles.Play();
            }

            //Shield invincibility
            StartCoroutine("SubtractShieldHealthCooldown");

            //Shield hit sound

            //Set shieldIsHit to false
            shieldIsHit = false;
        }
    }

    private IEnumerator SubtractShieldHealthCooldown()
    {
        shieldHealthInvincibility = true;
        yield return new WaitForSeconds(0.1f);
        shieldHealthInvincibility = false;
    }
}
