using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the parent character class which all other character classes inherit from
public abstract class PlayerCharacter : MonoBehaviour
{
    private Rigidbody rb;
    protected float walkSpeed;
    protected int knockbackPower;

    private Camera mainCam;

    private LayerMask aimArea;

    [HideInInspector]
    public static bool isInvincible;
    private bool isGettingKnockedback;

    public delegate void PlayerControl();
    public static event PlayerControl PlayerControls;

    //Happens when a character gets enabled
    protected virtual void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        mainCam = Camera.main;

        //Subscribes main functions to PlayerControls
        PlayerControls += Movement;
        PlayerControls += Aim;

        //Sets aimArea layer to layer #10 (AimArea)
        aimArea = 1 << 10;
    }

    //Happens when a character gets disabled
    private void OnDisable()
    {
        //Unsubscribes main functions from PlayerControls
        PlayerControls -= Movement;
        PlayerControls -= Aim;
    }

    void Start()
    {
        //Reset at start of every round since it's a static variable
        isInvincible = false;
    }

    void Update()
    {
        //Calls all the functions subscribed to the PlayerControls event every frame.
        PlayerControls();
    }

    //Movement
    protected virtual void Movement()
    {
        //Sets the horizontal movement value to moveHorizontal and the vertical movement value to moveVertical
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        //Changes the velocity of the character depending on their move direction multiplied by their speed.
        //Disable player-controlled movement while they're getting knockedback
        if (!isGettingKnockedback)
        {
            rb.velocity = new Vector3(moveHorizontal, 0, moveVertical).normalized * walkSpeed;
        }     

        if (moveHorizontal != 0 || moveVertical != 0 && !isGettingKnockedback)
        {
            //Make weapon bounce
            //Put an animation on this script and make it call the PlayerWalk trigger, then be sure to have an animator on every player character which has a trigger and animation for that
        }

        //Ground Check
        if (!GroundCheck.playerIsTouchingGround)
        {
            //Add downward force while not touching ground so that the player falls.
            rb.AddForce((Vector3.down * 800), ForceMode.Acceleration);
        }
    }

    //Character aims where the player's mouse is aiming
    void Aim()
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
    //Character takes more or less damage depending on which character they are.
    //Takes in parameters "damageReceived" which is how much damage is coming in, and "hitFrom" which is the position the enemy is getting hitfrom to calculate the knockback direction
    public virtual void TakeDamage(int damageReceived, Vector3 hitFrom)
    {
        //Subtract player health by the damage received
        PlayerHealth.playerHealth -= damageReceived;
        
        //Do the following only if player is still alive
        if (PlayerHealth.playerHealth > 0)
        {
            //Knock player back
            rb.AddForce((transform.position - hitFrom).normalized * knockbackPower, ForceMode.Acceleration);

            //Short invincibility after getting hit
            StartCoroutine("Invincibility");

            //Play sound fx

            //Show player getting hurt and show health being lost
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

    //Abstract function used to deal damage to an enemy. In the child classes it'll take in an EnemyCharacter script.
    public abstract void DealDamage <T>(T component) where T : Component;
}
