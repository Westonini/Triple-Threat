using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the parent character class which all other character classes inherit from
public abstract class PlayerCharacter : MonoBehaviour
{
    private Rigidbody rb;
    protected float walkSpeed;
    private Vector3 movement;

    private Camera mainCam;

    public static int playerHealth = 10;

    private LayerMask aimArea;

    public delegate void PlayerControl();
    public static event PlayerControl PlayerControls;

    //Happens when a character gets enabled
    private void OnEnable()
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
        rb.velocity = new Vector3(moveHorizontal, 0, moveVertical).normalized * walkSpeed;

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
    protected virtual void TakeDamage(int damageReceived)
    {
        playerHealth -= damageReceived;

        //Knock player back

        //Play sound fx

        //Show player getting hurt and show health being lost
    }

    //Each character can perform a different action.
    protected abstract void Action <T>(T component) where T : Component;
}
