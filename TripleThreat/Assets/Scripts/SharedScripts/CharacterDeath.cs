using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent class for PlayerDeath and EnemyDeath
public abstract class CharacterDeath : MonoBehaviour
{
    protected Rigidbody characterRB;
    protected CharacterMovementAnimations animationsScript;
    protected Animator characterAnim;

    public GameObject smokeParticles;
    protected List<string> possibleSmokeSounds = new List<string>();

    private void Start()
    {
        //Add swing sounds to the possibleSounds list.
        possibleSmokeSounds.Add("SmokePoof1"); possibleSmokeSounds.Add("SmokePoof2");
    }

    //To be invoked when _playerDied/_enemyDied gets invoked from PlayerCharacter/EnemyCharacter
    protected void HandleDeath()
    {
        StartCoroutine("DeathIEnum");
    }
}
