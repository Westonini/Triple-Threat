using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the parent character class which all other player AND enemy character classes inherit from
public abstract class Character : MonoBehaviour
{
    [HideInInspector] public Rigidbody rb;    //Character's rigidbody component
    public float walkSpeed;                   //Character's walk speed value
    protected float defaultWalkSpeed;         //Gets set to walkspeed at Start() so that walkSpeed can be reset later
    [Space]
    public GameObject bloodParticles;         //BloodParticles gameobject
    public float bloodParticlesYOffset;       //Amount that the BloodParticles will be offset by on the y-axis

    protected float downwardForce;            //Amount of force that will push the character downward for gravity simulation
    protected bool isTouchingGround;          //Boolean to check if player is touching ground
    protected bool isDying;                                   //Set to true if dying, otherwise it'll be false

    public delegate void CharacterFalling();
    public event CharacterFalling _characterFalling;             //Event to be invoked when the character starts falling

    public delegate void CharacterLanded();
    public event CharacterFalling _characterLanded;             //Event to be invoked when the character lands

    protected CharacterMovementAnimations animationsScript;

    protected virtual void Start()
    {
        //Get components       
        rb = GetComponent<Rigidbody>();
        animationsScript = GetComponent<CharacterMovementAnimations>();

        //Set defaultWalkSpeed to walkSpeed so it can be reset later.
        defaultWalkSpeed = walkSpeed;
    }

    protected virtual void Update()
    {
        MovementAnimationInput();
        Aim();
    }

    protected virtual void FixedUpdate()
    {
        Movement();
    }

    //Movement
    protected virtual void Movement()
    {
        //Ground Check
        if (!isTouchingGround)
        {
            //Add downward force while not touching ground so that the character falls.
            rb.AddForce(Vector3.down * downwardForce, ForceMode.Acceleration);
        }
    }

    protected virtual void MovementAnimationInput()
    {
        //To be overwritten within PlayerCharacter and EnemyCharacter
    }
    protected virtual void Aim()
    {
        //To be overwritten within PlayerCharacter and EnemyCharacter
    }


    //Ground Check
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isTouchingGround = true;

            if (_characterLanded != null)
            {
                _characterLanded.Invoke(); //Stops couroutine within CharacterLand
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isTouchingGround = false;

            if (_characterFalling != null)
            {
                _characterFalling.Invoke(); //Starts couroutine within CharacterLand
            }
        }
    }

    //Abstract function used to deal damage to a character. In the child classes it'll take in either a PlayerCharacter or EnemyCharacter script.
    public abstract void DealDamage<T>(T component) where T : Component;
}
