using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//This script starts the RestartScene() Coroutine when the gameOver boolean gets set to true from the PlayerCharacter script
public class SceneRestart : MonoBehaviour
{
    [HideInInspector]
    public bool sceneCurrentlyRestarting;

    private void OnEnable()
    {
        PlayerDeath._playerDead += HandleSceneRestart;
    }
    private void OnDisable()
    {
        PlayerDeath._playerDead -= HandleSceneRestart;
    }

    void HandleSceneRestart()
    {
        if (!sceneCurrentlyRestarting)
        {
            StartCoroutine("RestartScene");
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

        //Wait x seconds and then restart the current scene.
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
