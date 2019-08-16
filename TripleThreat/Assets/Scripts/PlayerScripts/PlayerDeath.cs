
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player death coroutine.
public class PlayerDeath : CharacterDeath //Inherits from CharacterDeath
{
    PlayerCharacter playerScript;

    public Animator redTintAnim;

    public delegate void PlayerDead();
    public static event PlayerDead _playerDead;             //Event to be invoked when the player finished their dying state and has died.

    void OnEnable()
    {
        PlayerCharacter._playerDying += HandleDeath;
    }

    private void OnDisable()
    {
        PlayerCharacter._playerDying -= HandleDeath;
    }

    //Disable the player's PlayerCharacter and SwapCharacter scripts and get rid of constraints on the player's rigidbody. 
    //Afterwards set the restartscene boolean from the SceneRestart script to true.
    IEnumerator DeathIEnum()
    {
        //Get objects/components
        characterRB = SwapCharacters.currentCharacter.GetComponent<Rigidbody>();
        playerScript = SwapCharacters.currentCharacter.GetComponent<PlayerCharacter>();
        characterAnim = SwapCharacters.currentCharacter.GetComponent<Animator>();
        animationsScript = SwapCharacters.currentCharacter.GetComponent<CharacterMovementAnimations>();

        //Set all gameobjects tagged "Equipment" to inactive.
        foreach (Transform equipment in SwapCharacters.currentCharacter.transform) if (equipment.CompareTag("Equipment"))
            {
                equipment.gameObject.SetActive(false);
            }

        //Enable/disable things
        characterAnim.enabled = false;
        redTintAnim.SetTrigger("Activate");
        animationsScript.ToggleDustParticles(false);
        playerScript.enabled = false;

        //Rigidbody constraint changes for a ragdoll-type effect
        characterRB.constraints = RigidbodyConstraints.None;
        characterRB.constraints = RigidbodyConstraints.FreezeRotationY;
        characterRB.mass = 100;

        //Play sound
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        //If the player character is active, instantiate blood particles every so often and end it with a smoke particle.
        if (SwapCharacters.currentCharacter.activeSelf)
        {
            yield return new WaitForSeconds(1.5f);
            InstantiateParticles.InstantiateParticle(SwapCharacters.currentCharacter.transform, playerScript.bloodParticles, 5f, 2.5f);
            yield return new WaitForSeconds(1.5f);
            InstantiateParticles.InstantiateParticle(SwapCharacters.currentCharacter.transform, playerScript.bloodParticles, 5f, 2.5f);
            yield return new WaitForSeconds(1.5f);
            InstantiateParticles.InstantiateParticle(SwapCharacters.currentCharacter.transform, smokeParticles, 2f, 0.25f);
        }
        //If the player character was inactive due to them falling off the map or some othe reason, only wait for 1.7 seconds
        else
        {
            yield return new WaitForSeconds(1.7f);
        }

        //Will start the RestartScene coroutine within the SceneRestart script
        _playerDead.Invoke();
    }
}
