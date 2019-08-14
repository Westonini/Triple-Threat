using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementAnimations : MonoBehaviour
{
    public ParticleSystem dustParticles;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    //Toggles the character's walk animation on or off depending on the boolean passed in.
    public void ToggleWalkAnimation(bool OnOff)
    {
        if (OnOff)
        {
            anim.SetTrigger("Walk");
        }
        else
        {
            anim.ResetTrigger("Walk");
        }
    }

    //Toggles the character's dust particles depending on the boolean passed in.
    public void ToggleDustParticles(bool OnOff)
    {
        if (OnOff && !dustParticles.isEmitting)
        {
            dustParticles.Play();
        }
        else if (!OnOff)
        {
            dustParticles.Stop();
        }
    }

    public void ChangeDustParticlesSpeed(float speed)
    {
        //Get the main module of the dustParticles to change the speed in the following code
        var main = dustParticles.main;

        main.startSpeed = speed;
    }
}
