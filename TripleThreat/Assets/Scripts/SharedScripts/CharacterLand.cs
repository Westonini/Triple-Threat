﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used for when a character is falling and lands after a long period
public class CharacterLand : MonoBehaviour
{
    float fallingTimeElapsed;
    public GameObject landingParticles;
    public string soundName = "Landing_Normal";

    Character characterScript;

    void Start()
    {
        //If the ground check is tagged "PlayerGC", meaning it's a player, get the currentCharacter Character script.
        //otherwise if it's not tagged "PlayerGC", meaning it's an enemy or NPC, get the Character script from the parent object.
        characterScript = gameObject.tag == "PlayerGC" ? characterScript = SwapCharacters.currentCharacter.GetComponent<Character>() : characterScript = GetComponent<Character>();

        //Subscribe functions to events in the character script
        characterScript._characterFalling += StartFall;
        characterScript._characterLanded += Landed;
    }

    void StartFall()
    {
        StartCoroutine("Falling");
    }

    //Every 0.5 seconds add 0.5 seconds to the fallingTimeElapsed variable.
    IEnumerator Falling()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            fallingTimeElapsed += 0.5f;
        }
    }

    void Landed()
    {
        //Stop the Falling coroutine
        StopCoroutine("Falling");
        //If the character was falling for at least 1 second, instantiate a landing particle
        if (fallingTimeElapsed >= 1f)
        {
            InstantiateParticles.InstantiateParticle(transform, landingParticles, 5f, 1f);
            //audioManager.Play("Landing");
            FindObjectOfType<AudioManager>().Play(soundName, transform);
        }

        //Set fallingTimeElapsed to 0
        fallingTimeElapsed = 0;
    }
}
