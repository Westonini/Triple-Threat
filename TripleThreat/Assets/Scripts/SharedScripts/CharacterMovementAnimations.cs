using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementAnimations : MonoBehaviour
{
    ParticleSystem dustParticles;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CharacterWalkAnimation()
    {
        /*//Walk/Idle animations
        //If the player is currently moving and is not getting knocked back, do walk animation
        if (moveHorizontal != 0 || moveVertical != 0 && !isGettingKnockedback)
        {
            anim.SetTrigger("Walk");
        }
        //Else stop the trigger.
        else
        {
            anim.ResetTrigger("Walk");
        }


        //Dust particles show up while the player is moving or getting knocked back while grounded.
        if (((moveHorizontal != 0 || moveVertical != 0) || isGettingKnockedback) && isTouchingGround)
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
        }*/
    }
}
