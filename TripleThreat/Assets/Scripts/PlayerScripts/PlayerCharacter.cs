using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the parent character class which all other player character classes inherit from
public abstract class PlayerCharacter : Character //Inherits from Character
{
    [Space]
    public int knockbackPower;                              //The amount which this character gets knocked back by

    float moveHorizontal;                                   //Will be set to 1 if going right and -1 if going left
    float moveVertical;                                     //Will be set to 1 if going up and -1 if going down

    private Camera mainCam;                                 //Main camera

    private LayerMask aimArea;                              //Aim area layer that the player will be able to put their mouse on in order to aim

    [HideInInspector] public static bool isInvincible;      //Set to true if invincible, otherwise it'll be false. static because it's for all 3 player characters.
    private bool isGettingKnockedback;                      //Set to true if getting knocked back, otherwise it'll be false

    public delegate void PlayerTookDmg();                   
    public static event PlayerTookDmg _playerTookDmg;       //Event to be invoked when the player takes damage

    public delegate void PlayerDied();
    public static event PlayerDied _playerDying;             //Event to be invoked when the player dies

    //Happens when a character gets enabled
    protected virtual void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        //Get layermask
        aimArea = LayerMask.GetMask("AimArea");
    }

    protected override void Start()
    {
        //Reset at start of every round since it's a static variable
        isInvincible = false;

        downwardForce = 800;

        base.Start();
    }

    //Movement
    protected override void Movement()
    {
        //Sets the horizontal movement value to moveHorizontal and the vertical movement value to moveVertical
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        //Changes the velocity of the character depending on their move direction multiplied by their speed.
        //Disable player-controlled movement while they're getting knockedback
        if (!isGettingKnockedback)
        {
            rb.velocity = new Vector3(moveHorizontal, 0, moveVertical).normalized * walkSpeed;
        }

        base.Movement();
    }

    protected override void MovementAnimationInput()
    {
        //Movement animations and dust particles
        //If the player is currently moving and touching the ground..
        if ((moveHorizontal != 0 || moveVertical != 0) && isTouchingGround)
        {
            //Emit dust particles.
            animationsScript.ToggleDustParticles(true);

            //If the player isn't getting knocked back, do walk animation.
            if (!isGettingKnockedback)
            {
                animationsScript.ToggleWalkAnimation(true);
            }
        }
        //Else stop movement animations and dust particles
        else
        {
            animationsScript.ToggleWalkAnimation(false);
            animationsScript.ToggleDustParticles(false);
        }
    }

    //Character aims where the player's mouse is aiming
    protected override void Aim()
    {
        //Move character's y rotation to look at where mouse is pointing
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        //If character's mouse is on the AimArea layer...
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, aimArea))
        {
            Vector3 lookAtPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            transform.LookAt(lookAtPosition);
        }
    }

    //TakeDamage is to be called in Enemy classes for when they hit a player.
    //Character takes more or less damage depending on their knockbackPower value.
    //Takes in parameters "damageReceived" which is how much damage is coming in, and "hitFrom" which is the position the enemy is getting hitfrom to calculate the knockback direction
    public virtual void TakeDamage(int damageReceived, Vector3 hitFrom)
    {
        //Deal damage and instantiate blood particles damage received is above 0 damage
        if (damageReceived > 0 && PlayerHealth.playerHealth > 0)
        {
            PlayerHealth.playerHealth -= damageReceived;
            InstantiateParticles.InstantiateParticle(transform, bloodParticles, 5f, 5f);
        }

        //Play sound fx

        //Invoke _playerTookDmg to update health text
        if (_playerTookDmg != null)
            _playerTookDmg();

        //Invoke _playerdied and do a ragdoll knockback if the player health dropped to 0 or below after they got hit.
        if (PlayerHealth.playerHealth <= 0 && !isDying)
        {
            //Invoke _playerDying
            if (_playerDying != null)
                _playerDying();

            //Ragdoll death knock back
            rb.AddForce((transform.position - hitFrom).normalized * (knockbackPower / 2), ForceMode.Acceleration);

            //Set isDying to true so this can't happen multiple times.
            isDying = true;
        }
        //Else if the player's health is above 0 after they got hit do a regular knockback and start the invincibility coroutine
        else if (PlayerHealth.playerHealth > 0)
        {
            //Regular player knock back
            rb.AddForce((transform.position - hitFrom).normalized * knockbackPower, ForceMode.Acceleration);

            //Short invincibility after getting hit
            StartCoroutine("Invincibility");
        }
    }

    //Short invincibility after getting hit
    //Also sets isGettingKnockedback to true for a short duration to prevent player from moving during knockback
    private IEnumerator Invincibility()
    {
        isGettingKnockedback = true;
        isInvincible = true;
        yield return new WaitForSeconds(0.15f);
        isGettingKnockedback = false;
        yield return new WaitForSeconds(0.10f);
        isInvincible = false;
    }
}

