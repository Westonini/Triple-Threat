﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script keeps track of the player health and updates the player health text accordingly
public class PlayerHealthText : MonoBehaviour
{
    TextMeshProUGUI healthText;
    Animator animator;

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
        animator = GetComponent<Animator>();
    }

    //Updates the healthtext's number and color depending on the player's health
    public void HealthText()
    {
        //HealthText value update
        healthText.text = "HP: " + PlayerHealth.playerHealth.ToString() + " / 10";

        //Flash the health text if the player was hurt
        if (PlayerHealth.playerHealth < 10)
        {
            animator.SetTrigger("Flash");
        }

        //HealthText color
        if (PlayerHealth.playerHealth >= 6)
        {
            //Green health color
            healthText.color = new Color32(0, 255, 0, 200);
        }
        else if (PlayerHealth.playerHealth < 6 && PlayerHealth.playerHealth >= 3)
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

