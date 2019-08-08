using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script keeps track of the player health and updates the player health text accordingly
public class PlayerHealth : MonoBehaviour
{
    TextMeshProUGUI healthText;

    public static int playerHealth = 10;
    public static bool gameOver;

    void Start()
    {
        //Reset the static variable, playerHealth, at the start of each scene
        playerHealth = 10;
        gameOver = false;

        healthText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        //Calls the HealthText function every frame
        HealthText();

        //If playerHealth drops below 0 set it to 0
        if (playerHealth < 0)
        {
            playerHealth = 0;
        }

        //Kill the player and restart scene if their health is <= 0
        if (playerHealth <= 0)
        {
            //Sets gameOver to true, destroying all objects tagged "Player" and restarting the scene within the SceneRestart gameobject.
            gameOver = true;
        }
    }

    //Updates the healthtext's number and color depending on the player's health
    void HealthText()
    {
        //HealthText value update
        healthText.text = "HP: " + playerHealth.ToString() + " / 10";


        //HealthText color
        if (playerHealth >= 6)
        {
            //Green health color
            healthText.color = new Color32(0, 255, 0, 200);
        }
        else if (playerHealth < 6 && playerHealth >= 3)
        {
            //Yellow health color
            healthText.color = new Color32(255, 255, 0, 200);
        }
        else
        {
            //Red health color
            healthText.color = new Color32(255, 0, 0, 200);
        }
    }
}
