using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Perform a melee swing which will damage any enemies caught in the attack.
public class WarriorAction : MonoBehaviour
{
    Warrior warriorScript;

    private Collider swordCollider;
    private Animator swordAnim;
    private TrailRenderer swordTrail;

    [HideInInspector] public EnemyCharacter enemyHit;
    [HideInInspector] public bool currentlyAttacking, attack1Used, attack2Used;

    List<string> possibleSwingSounds = new List<string>();

    private void Start()
    {
        //Get the sword's collider, animator, and trail.
        swordCollider = GetComponent<Collider>();
        swordAnim = GetComponent<Animator>();
        swordTrail = GetComponentInChildren<TrailRenderer>();

        //Get the Warrior script from the parent object
        warriorScript = GetComponentInParent<Warrior>();

        //Added so that the animations don't bug.
        swordAnim.keepAnimatorControllerStateOnDisable = true;

        //Add swing sounds to the possibleSounds list.
        possibleSwingSounds.Add("Swing1"); possibleSwingSounds.Add("Swing2"); possibleSwingSounds.Add("Swing3"); possibleSwingSounds.Add("Swing4"); possibleSwingSounds.Add("Swing5"); possibleSwingSounds.Add("Swing6");
    }

    //Reset these things when the player swaps out of this character.
    private void OnDisable()
    {
        StopCoroutine("WarriorAttack");
        swordAnim.SetBool("Attack1", false);
        swordAnim.SetBool("Attack2", false);

        swordCollider.enabled = false;
        currentlyAttacking = false;
        swordTrail.enabled = false;
    }

    private void Update()
    {
        //If the player presses the "Fire1" key, stop then start the WarriorAttack coroutine
        //Can only attack if attack isn't currently on cooldown
        if (Input.GetButtonDown("Fire1") && !currentlyAttacking)
        {
            StopCoroutine("WarriorAttack");
            StartCoroutine(WarriorAttack("Attack1", 0.1f, 0.33f, 0.325f));
        }

        if (Input.GetButtonDown("Fire2") && !currentlyAttacking)
        {
            StopCoroutine("WarriorAttack");
            StartCoroutine(WarriorAttack("Attack2", 0.25f, 0.915f, 0.84f));
        }
    }

    //If the sword touched an enemy, set that enemy's gameobject to enemyHit and  call DealDamage with enemyHit as the parameter passed in.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (swordAnim.GetBool("Attack1"))
                attack1Used = true;
            if (swordAnim.GetBool("Attack2"))
                attack2Used = true;
            enemyHit = other.gameObject.GetComponent<EnemyCharacter>();
            warriorScript.DealDamage(enemyHit);
        }
    }

    //Enable the sword's collider and play the sword swing animation
    private IEnumerator WarriorAttack(string move, float attackTime, float fullAnimationTime, float cooldownTime)
    {
        swordCollider.enabled = true;    //Enable collider
        swordAnim.SetBool(move, true);   //Play animation
        swordTrail.enabled = true;       //Enable weapon trail       
        FindObjectOfType<AudioManager>().PlayRandom(possibleSwingSounds);  //Play random swing sound
        currentlyAttacking = true;      //Set currentlyAttacking bool to true (for use in Warrior script)

        yield return new WaitForSeconds(attackTime); //Time until the attack portion ends

        swordCollider.enabled = false;  //Disable collider
        swordTrail.enabled = false;     //Disable weapon trail

        yield return new WaitForSeconds((fullAnimationTime - attackTime));

        swordAnim.SetBool(move, false);  //Stop the animation
        currentlyAttacking = false;     //Set currentlyAttacking bool to false
    }
}
