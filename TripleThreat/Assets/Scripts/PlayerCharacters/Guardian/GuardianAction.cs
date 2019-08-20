using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Hold left mouse button to block attacks and push attackers back. 
public class GuardianAction : MonoBehaviour
{
    Guardian guardianScript;
    GuardianChargeCooldown chargeCooldownScript;

    private GameObject playerMain;

    private Collider shieldCollider;
    private Animator shieldAnim;
    private MeshRenderer shieldRenderer;
    private ParticleSystem hitParticles;
    public GameObject chargeTrails;
    public ParticleSystem sparkParticle;

    [HideInInspector] public EnemyCharacter enemyHit;

    [HideInInspector] public bool currentlyBashing;
    [HideInInspector] public bool currentlyCharging;
    public static bool chargeOnCooldown;

    private float chargedCooldownContinueTimer;

    public static int shieldHealth = 10;
    public static bool shieldHealthInvincibility;

    public delegate void ShieldGotHit();
    public static event ShieldGotHit _shieldHit;             //Event to be invoked when the shield gets hit

    List<string> possibleBashSounds = new List<string>();
    List<string> possibleChargeSounds = new List<string>();
    List<string> possibleBlockSounds = new List<string>();

    private void OnEnable()
    {
        //Subscribes functions to events
        _shieldHit += ShieldBreak;
        _shieldHit += ShieldHit;
        ShieldRepair._shieldRepaired += EnableShield;
    }

    private void OnDisable()
    {
        //Unsubscribes functions from events
        _shieldHit -= ShieldHit;
        _shieldHit -= ShieldBreak;

        //Sets some things to false
        currentlyBashing = false;
        currentlyCharging = false;

        shieldHealthInvincibility = false;
        StopCoroutine("SubtractShieldHealthCooldown");
        StopCoroutine("ShieldBashCooldown");

        chargeTrails.SetActive(false);

        guardianScript.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
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
        playerMain = GameObject.FindGameObjectWithTag("PlayerMain");
        playerMain.AddComponent<GuardianChargeCooldown>(); //Add the GuardianChargeCooldown script to the PlayerMain object
        chargeCooldownScript = GameObject.FindGameObjectWithTag("PlayerMain").GetComponent<GuardianChargeCooldown>(); //Gets the GuardianChargeCooldown script from the PlayerMain object.

        //Set static booleans to false on the start of the scene
        shieldHealthInvincibility = false;

        //This is to make sure some animations don't bug out.
        shieldAnim.keepAnimatorControllerStateOnDisable = true;

        //Add block sounds to the possibleSounds list.
        possibleBlockSounds.Add("Block1"); possibleBlockSounds.Add("Block2"); possibleBlockSounds.Add("Block3"); possibleBlockSounds.Add("Block4");
        //Add bash sounds to the possibleSounds list.
        possibleBashSounds.Add("ShieldBash1"); possibleBashSounds.Add("ShieldBash2"); possibleBashSounds.Add("ShieldBash3");
        //Add charge sounds to the possibleSounds list.
        possibleChargeSounds.Add("Swing3"); possibleChargeSounds.Add("Swing4");
    }

    void Update()
    {
        //Shield Bash
        //When the player holds the Fire1 key play the shield bash animation and set the currentlyblocking boolean to true
        if (Input.GetButton("Fire1") && shieldHealth > 0 && !currentlyBashing && !currentlyCharging)
        {
            //Play shield bash animation
            shieldAnim.SetTrigger("Bash");

            //Play random shield bash sound
            FindObjectOfType<AudioManager>().PlayRandom(possibleBashSounds);

            //Start shield bash cooldown
            StartCoroutine("ShieldBashCooldown");
        }


        //Shield Charge
        //Do a charge forward if the player presses the Fire2 button
        if (Input.GetButtonDown("Fire2") && shieldHealth > 0 && !currentlyBashing && !currentlyCharging && !chargeOnCooldown)
        {
            //Start shield charge 
            StartCoroutine("ShieldCharge");
        }
    }


    IEnumerator ShieldCharge()
    {
        currentlyCharging = true;       //Set currentlyCharging to true to stop Aim() and Movement()
        chargeTrails.SetActive(true);   //Turn trails on
        sparkParticle.Play();           //Play a particle
        FindObjectOfType<AudioManager>().PlayRandom(possibleChargeSounds);   //Play random shield bash sound

        guardianScript.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ; //Freeze all rotation
        guardianScript.rb.AddRelativeForce(Vector3.forward * 1000, ForceMode.Acceleration);  //short charge

        yield return new WaitForSeconds(0.2f);

        chargeTrails.SetActive(false);  //Turn trails off
        currentlyCharging = false;      //Set currentlyCharging to false to allow Aim() and Movement() again.
        guardianScript.rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; //Reset to regular constraints
        chargeCooldownScript.StartChargeCooldown(); //Put charge on cooldown globally.
    }

    void ShieldBreak()
    {
        //If the shield's hp drops under 0 the shield becomes unusable until repaired.
        if (shieldHealth <= 0)
        {
            shieldAnim.ResetTrigger("Bash");
            shieldCollider.enabled = false;
            shieldRenderer.enabled = false;
            currentlyBashing = false;

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

        //Play random shield block sound when shield takes damage
        FindObjectOfType<AudioManager>().PlayRandom(possibleBlockSounds);
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

    private IEnumerator SubtractShieldHealthCooldown()
    {
        shieldHealthInvincibility = true;
        yield return new WaitForSeconds(0.5f);
        shieldHealthInvincibility = false;
    }

    //If the shieldbash animation is playing, set currentlyBashing to true. Otherwise set it false.
    //Play this after the shield charge for a 5 second cooldown.
    private IEnumerator ShieldBashCooldown()
    {
        currentlyBashing = true;
        yield return new WaitForSeconds(0.6f);
        currentlyBashing = false;
    }

    //If the shield touches an enemy, pass their gameobject in as a parameter in DealDamage in order to deal knockback
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyHit = collision.gameObject.GetComponent<EnemyCharacter>();
            guardianScript.DealDamage(enemyHit);

            //If the enemy is a FistEnemy type, subtract a shield point when touching them.
            if (!shieldHealthInvincibility && collision.gameObject.GetComponent<FistEnemy>())
            {
                SubtractShieldHealth(1);
                return;
            }

            //Play random shield block sound when touching an Enemy (but not a FistEnemy)
            FindObjectOfType<AudioManager>().PlayRandom(possibleBlockSounds);

            //Hit particle when touching an Enemy (but not a FistEnemy)
            if (!hitParticles.isEmitting)
            {
                hitParticles.Play();
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
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyHit = other.gameObject.GetComponent<EnemyCharacter>();
            guardianScript.DealDamage(enemyHit);

            //If the enemy is a FistEnemy type, subtract a shield point when touching them.
            if (!shieldHealthInvincibility && other.gameObject.GetComponent<FistEnemy>())
            {
                SubtractShieldHealth(1);
                return;
            }

            //Play random shield block sound when touching an Enemy (but not a FistEnemy)
            FindObjectOfType<AudioManager>().PlayRandom(possibleBlockSounds);

            //Hit particle when touching an Enemy (but not a FistEnemy)
            if (!hitParticles.isEmitting)
            {
                hitParticles.Play();
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyHit = other.gameObject.GetComponent<EnemyCharacter>();
            guardianScript.DealDamage(enemyHit);

            //If the enemy is a FistEnemy type, subtract a shield point when touching them.
            if (!shieldHealthInvincibility && other.gameObject.GetComponent<FistEnemy>())
            {
                SubtractShieldHealth(1);
            }
        }
    }
}
