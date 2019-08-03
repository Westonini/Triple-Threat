using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapCharacters : MonoBehaviour
{
    public GameObject character1;
    public GameObject character2;
    public GameObject character3;

    private GameObject currentCharacter;
    private Vector3 currentPosition;

    void Start()
    {
        //Start the level with character1
        currentCharacter = character1;
    }

    //Calls the Swap function to swap between characters
    void Update()
    {
        //If the player presses the "Chraracter1" Key and is not currently character 1, swap to character1.
        if (Input.GetButtonDown("Character1") && currentCharacter != character1)
        {
            Swap(ref character1);
        }
        //If the player presses the "Chraracter2" Key and is not currently character 2, swap to character2.
        if (Input.GetButtonDown("Character2") && currentCharacter != character2)
        {
            Swap(ref character2);
        }
        //If the player presses the "Chraracter3" Key and is not currently character 3, swap to character3.
        if (Input.GetButtonDown("Character3") && currentCharacter != character3)
        {
            Swap(ref character3);
        }
    }

    //Quickly sets all characters inactive before swapping to desired character.
    void Swap(ref GameObject character)
    {
        //Sets the current position as the transform of the currently used character.
        currentPosition = currentCharacter.transform.position;

        //Disable all characters
        character1.SetActive(false);
        character2.SetActive(false);
        character3.SetActive(false);

        //Enable the desired character and set that character's position to that of the previously used character's. Set current character to the character that was just swapped to.
        character.SetActive(true);
        character.transform.position = currentPosition;
        currentCharacter = character;
    }
}
