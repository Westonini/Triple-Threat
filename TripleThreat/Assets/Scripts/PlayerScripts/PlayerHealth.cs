using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls and contains the player health.
public class PlayerHealth : MonoBehaviour
{
    public static int playerHealth = 10;

    void Start()
    {
        //Reset the static variable, playerHealth, at the start of each scene
        playerHealth = 10;
    }

    void OnEnable()
    {
        PlayerCharacter._playerTookDmg += SetHealthToZero;
    }

    void OnDisable()
    {
        PlayerCharacter._playerTookDmg -= SetHealthToZero;
    }

    //Makes sure that if the player health can only be as low as zero.
    void SetHealthToZero()
    {
        playerHealth = playerHealth <= 0 ? 0 : playerHealth;
    }
}
