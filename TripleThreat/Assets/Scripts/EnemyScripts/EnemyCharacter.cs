using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the parent character class which all other enemy character classes inherit from
public abstract class EnemyCharacter : Character //Inherits from Character
{
    [Space]
    public int health;                                    //Enemy's health
    public int damage;                                    //Enemy's damage value
    public int defense;                                   //Enemy's defense value
    [Range(0, 100)] public int knockbackResistPercentage; //Percentage of knockback resistance against the player's knockback

    [HideInInspector] public bool isInvincible;           //Set to true if invincible, otherwise it'll be false

    protected override void Start()
    {
        downwardForce = 3;

        base.Start();
    }

    //Movement
    protected override void Movement()
    {
        //Enemy Movement Speed
        float step = walkSpeed * Time.deltaTime;

        //Enemy Movement Towards Player
        transform.position = Vector3.MoveTowards(transform.position, SwapCharacters.currentPlayerPosition.position, step);

        base.Movement();
    }

    protected override void MovementAnimations()
    {
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

        //Dust particles show up while the enemy is touching the ground and their velocity is at least 0
        if (rb.velocity.x >= 0 && isTouchingGround)
        {
            if (!dustParticles.isEmitting)
            {
                dustParticles.Play();
            }
        }
        //Otherwise stop them from showing up.
        else
        {
            dustParticles.Stop();
        }
    }

    protected override void Aim()
    {
        //Look at the player
        transform.LookAt(new Vector3(SwapCharacters.currentPlayerPosition.position.x, this.transform.position.y, SwapCharacters.currentPlayerPosition.position.z));
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
        health -= damageToDeal;

        //Knockback
        //The knockbackPower is passed in as a parameter by a player character during an attack.
        //The knockback resistance differs with each enemy and is calculated as percentage. For example: knockback power is 200, knockback resistance is 50% so they'll only get knocked back by 100 instead of 200.
        float calculatedKnockbackResistance = ((knockbackPower * knockbackResistPercentage) / 100);
        float calculatedKnockback = knockbackPower - calculatedKnockbackResistance;

        if (health > 0)
        {
            //Short invincibility after getting hit
            StartCoroutine("Invincibility");

            //Adds the knockback direction and amount
            rb.AddForce((transform.position - hitFrom).normalized * calculatedKnockback, ForceMode.Acceleration);
        }
        else
        {
            //Adds the knockback direction and amount
            rb.AddForce((transform.position - hitFrom).normalized * (calculatedKnockback / 2), ForceMode.Acceleration);
        }

        //Play sound fx

        //Instantiate blood particles when hurt
        if (damageToDeal > 0)
        {
            InstantiateParticles.InstantiateParticle(transform, bloodParticles, 5f, bloodParticlesYOffset);
        }
    }

    //Short invincibility after getting hit
    private IEnumerator Invincibility()
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.2f);
        isInvincible = false;
    }
}
