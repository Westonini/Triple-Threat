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
    public static bool shieldHealthInvincibility;

    public delegate void ShieldGotHit();
    public static event ShieldGotHit _shieldHit;             //Event to be invoked when the shield gets hit

    List<string> possibleBlockSounds = new List<string>();

    private void OnEnable()
    {
        //Subscribes functions to events
        _shieldHit += ShieldHit;
        _shieldHit += ShieldBreak;
        ShieldRepair._shieldRepaired += EnableShield;
    }

    private void OnDisable()
    {
        //Unsubscribes functions from events
        _shieldHit -= ShieldHit;
        _shieldHit -= ShieldBreak;

        //Sets some things to false
        currentlyBlocking = false;

        shieldHealthInvincibility = false;
        StopCoroutine("SubtractShieldHealthCooldown");
    }

    private void OnDestroy()
    {
        //Unsubscribes this only on destroy
        ShieldRepair._shieldRepaired -= EnableShield;
    }

    private void Start()
    {
        //Get components
        shieldCollider = GetComponent<Collider>();
        shieldAnim = GetComponent<Animator>();
        shieldRenderer = GetComponent<MeshRenderer>();
        hitParticles = GetComponentInChildren<ParticleSystem>();

        guardianScript = GetComponentInParent<Guardian>();

        //Set static booleans to false on the start of the scene
        shieldHealthInvincibility = false;

        //This is to make sure some animations don't bug out.
        shieldAnim.keepAnimatorControllerStateOnDisable = true;

        //Add block sounds to the possibleSounds list.
        possibleBlockSounds.Add("Block1"); possibleBlockSounds.Add("Block2"); possibleBlockSounds.Add("Block3"); possibleBlockSounds.Add("Block4");
    }

    void Update()
    {
        ShieldBash();
    }

    //If the shield touches an enemy, pass their gameobject in as a parameter in DealDamage in order to deal knockback
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyHit = collision.gameObject.GetComponent<EnemyCharacter>();
            guardianScript.DealDamage(enemyHit);

            //If the enemy is a FistEnemy type, subtract a shield point when touching them.
            if (!shieldHealthInvincibility && collision.gameObject.GetComponent<FistEnemy>())
            {
                SubtractShieldHealth(1);
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyHit = collision.gameObject.GetComponent<EnemyCharacter>();
            guardianScript.DealDamage(enemyHit);

            //If the enemy is a FistEnemy type, subtract a shield point when touching them.
            if (!shieldHealthInvincibility && collision.gameObject.GetComponent<FistEnemy>())
            {
                SubtractShieldHealth(1);
            }
        }
    }

    //A public static void that can subtract the shield health by any amount from any script
    public static void SubtractShieldHealth(int amount)
    {
        if (!shieldHealthInvincibility && amount != 10)
        {
            shieldHealth -= amount;
            if (_shieldHit != null)
                _shieldHit();
        }
        //If the amount of damage to deal is equal to 10, ignore the shieldinvincibility
        else
        {
            shieldHealth -= amount;
            if (_shieldHit != null)
                _shieldHit();
        }
    }

    //Do a shield bash if the player presses the Fire1 button
    void ShieldBash()
    {
        //When the player holds the Fire1 key play the shield bash animation and set the currentlyblocking boolean to true
        if (Input.GetButton("Fire1") && shieldHealth > 0)
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
        //If the shield's hp drops under 0 the shield becomes unusable until repaired.
        if (shieldHealth <= 0)
        {
            shieldAnim.ResetTrigger("Bash");
            shieldCollider.enabled = false;
            shieldRenderer.enabled = false;
            currentlyBlocking = false;

            //Make sure the lowest the health can go is 0.
            shieldHealth = 0;

            //Play shield break sound
            FindObjectOfType<AudioManager>().Play("ShieldBreak");
        }
    }

    //Enables the shield
    void EnableShield()
    {
        shieldCollider.enabled = true;
        shieldRenderer.enabled = true;
    }

    //When shield is hit play an impact particle, play a sound, and start the shield invincibility function
    void ShieldHit()
    {
        //Hit particle
        if (!hitParticles.isEmitting)
        {
            hitParticles.Play();
        }

        //Shield invincibility
        StartCoroutine("SubtractShieldHealthCooldown");

        //Play random shield block sound
        FindObjectOfType<AudioManager>().PlayRandom(possibleBlockSounds);
    }

    private IEnumerator SubtractShieldHealthCooldown()
    {
        shieldHealthInvincibility = true;
        yield return new WaitForSeconds(0.5f);
        shieldHealthInvincibility = false;
    }
}
