using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script shows the current characters in text that the player has access through via 1-3 keys.
public class CurrentCharactersUI : MonoBehaviour
{
    SwapCharacters SCscript;
    public TextMeshProUGUI currentCharactersText;

    void Start()
    {
        SCscript = GetComponent<SwapCharacters>();
    }

    void Update()
    {
        ShowCurrentCharacters();
    }

    //Get the character names from the SwapCharacter script and show them in text.
    void ShowCurrentCharacters()
    {
        currentCharactersText.text = "Character 1 - " + SCscript.character1.name + "\n" +
                                     "Character 2 - " + SCscript.character2.name + "\n" +
                                     "Character 3 - " + SCscript.character3.name;
    }
}
