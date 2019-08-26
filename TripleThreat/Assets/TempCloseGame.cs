using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//To be removed later. This closes the game if the player presses the Escape button
public class TempCloseGame : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
