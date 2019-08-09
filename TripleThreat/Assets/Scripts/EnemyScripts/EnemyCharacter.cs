using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyCharacter : MonoBehaviour
{
    private Rigidbody rb;

    protected float walkSpeed;
    protected int enemyHealth;
    protected int defense;
    protected int knockbackPower;
    protected int knockbackResistance;

    private GroundCheck groundCheck;

    [HideInInspector]
    public bool isInvincible;

    private Animator anim;

    protected virtual void Start()
    {
        //Get rigidbody and animator components from the object this script is attached to.
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        //Get a reference to this object's GroundCheck script by getting the component from its children.
        groundCheck = GetComponentInChildren<GroundCheck>();
    }

    private void Update()
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
        transform.LookAt(new Vector3(SwapCharacters.currentPlayerPosition.position.x, this.transform.position.y, SwapCharacters.currentPlayerPosition.position.z));

        //Ground Check
        if (!groundCheck.enemyIsTouchingGround)
        {
            //Add downward force while not touching ground so that the enemy falls.
            rb.AddForce(Vector3.down * 3, ForceMode.Acceleration);
        }

        //Walk/Idle animations
        //If enemy just got hit, stop walk animation while invincible.
        if (!isInvincible)
        {
            anim.SetTrigger("Walk");
        }
        else
        {
            anim.ResetTrigger("Walk");
        }
    }

    //TakeDamage is to be called in player classes for when they hit an enemy.
    //Enemies takes more or less damage depending on their defense.
    //Takes in parameters "damageReceived" which is how much damage is coming in, and "hitFrom" which is the position the enemy is getting hitfrom to calculate the knockback direction
    public virtual void TakeDamage(int damageReceived, Vector3 hitFrom, int knockbackPower)
    {
        //Will be used to hold the value of damage to be dealt to the enemy
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

        //Knockback
        //The knockbackPower is passed in as a parameter by a player character during an attack.
        //The knockback resistance differs with each enemy and is calculated as percentage. For example: knockback power is 200, knockback resistance is 50% so they'll only get knocked back by 100 instead of 200.
        float calculatedKnockbackResistance = ((knockbackPower * knockbackResistance) / 100);
        float calculatedKnockback = knockbackPower - calculatedKnockbackResistance;

        //Adds the knockback direction and amount
        rb.AddForce((transform.position - hitFrom).normalized * calculatedKnockback, ForceMode.Acceleration);

        //Short invincibility after getting hit
        StartCoroutine("Invincibility");

        //Play sound fx

        //Show enemy getting hurt
    }

    //Short invincibility after getting hit
    private IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
    }

    //Abstract function used to deal damage to a player's character. In the child classes it'll take in a PlayerCharacter script.
    public abstract void DealDamage<T>(T component) where T : Component;
}
