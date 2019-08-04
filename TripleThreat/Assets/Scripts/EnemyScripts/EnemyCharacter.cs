using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCharacter : MonoBehaviour
{
    private Rigidbody rb;

    protected int enemyHealth;
    protected int defense;
    protected float walkSpeed;

    private GroundCheck groundCheck;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Get a reference to this object's GroundCheck script by getting the component from its children.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    void Update()
    {
        Movement();
    }

    //Movement
    protected virtual void Movement()
    {
        //Enemy Movement Speed
        float step = walkSpeed * Time.deltaTime;

        //Enemy Movement Towards Player
        transform.position = Vector3.MoveTowards(transform.position, SwapCharacters.currentPlayerPosition.position, step);
        //Look at the player
        if (SwapCharacters.currentPlayerPosition.position.y == transform.position.y)
        {
            transform.LookAt(SwapCharacters.currentPlayerPosition.position);
        }


        //Ground Check
        if (!groundCheck.enemyIsTouchingGround)
        {
            //Add downward force while not touching ground so that the enemy falls.
            rb.AddForce(Vector3.down * 3, ForceMode.Acceleration);
        }
    }

    //TakeDamage is to be called in player classes for when they hit an enemy.
    //Enemies takes more or less damage depending on their defense.
    protected virtual void TakeDamage(int damageReceived)
    {
        int damageToDeal;

        //If the enemy's defense is higher than the damage they're receiving, deal 0 damage.
        if (defense >= damageReceived)
        {
            damageToDeal = 0;
        }
        //Otherwise set damageToDeal to the enemy subtracted by their defense amount.
        else
        {
            damageToDeal = (damageReceived - defense);
        }

        //Subtract enemyHealth by the damageToDeal
        enemyHealth -= damageToDeal;

        //Kill the enemy if their health is <= 0
        if (enemyHealth <= 0)
        {
            Destroy(gameObject);
        }

        //Knock enemy back

        //Play sound fx

        //Show enemy getting hurt
    }

    //Each enemy can perform a different action.
    protected abstract void Action<T>(T component) where T : Component;
}
