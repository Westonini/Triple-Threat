﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script starts the RestartScene() Coroutine when the gameOver boolean gets set to true from the PlayerCharacter script
public class SceneRestart : MonoBehaviour
{
    [HideInInspector]
    public bool sceneCurrentlyRestarting;

    private void Update()
    {
        //If gameOver gets set to true within the PlayerCharacter script, call the RestartScene Coroutine and set gameOver to false.
        if (PlayerHealth.gameOver)
        {
            StartCoroutine("RestartScene");
            PlayerHealth.gameOver = false;
        }
    }

    private IEnumerator RestartScene()
    {
        sceneCurrentlyRestarting = true;

        //Find all the playerCharacters and add them to an array.
        GameObject[] playerCharacters = GameObject.FindGameObjectsWithTag("Player");

        //Disable all objects tagged "Player"
        for (var i = 0; i < playerCharacters.Length; i++)
        {
            playerCharacters[i].SetActive(false);
        }

        //Wait 3 seconds and then restart the current scene.
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
