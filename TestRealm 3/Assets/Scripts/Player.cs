using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour
{
    private Rigidbody rb;
    public float walkSpeed = 5;
    private Vector3 movement;

    public static int playerHealth = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        //Movement
        rb.velocity = new Vector3(moveHorizontal, 0, moveVertical).normalized * walkSpeed;

        //Ground Check
        if (!PlayerGroundCheck.playerIsTouchingGround)
        {
            //Gravity increase while not touching ground so that the player falls.
            Physics.gravity = new Vector3(0, -1500f, 0);
        }
        else
        {
            //Gravity is normal while touching ground.
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }
    }

    protected void TakeDamage(int damageReceived)
    {
        playerHealth -= damageReceived;

        //Knock player back

        //Play sound fx

        //Show player getting hurt and show health being lost
    }

    protected abstract void Attack <T>(T component) where T : Component;

}
