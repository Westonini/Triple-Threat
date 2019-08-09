using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//This script shows the current characters in text that the player has access through via 1-3 keys.
public class CurrentCharactersUI : MonoBehaviour
{
    SwapCharacters SCscript;

    public TextMeshProUGUI character1Name;
    public TextMeshProUGUI character2Name;
    public TextMeshProUGUI character3Name;
    public Image character1Image;
    public Image character2Image;
    public Image character3Image;

    [Space]
    public Sprite Warrior;
    public Sprite Guardian;
    public Sprite Archer;

    private string character1;
    private string character2;
    private string character3;

    void Start()
    {
        SCscript = GetComponent<SwapCharacters>();

        ShowCurrentCharacters();
    }

    private void Update()
    {
        //if the character currently being used is x, call the function to brighten x's sprite
        if (SCscript.currentCharacter.name == character1)
        {
            BrightenCurrentlySelectedCharacterSprite(character1Image, character1Name);
        }
        if (SCscript.currentCharacter.name == character2)
        {
            BrightenCurrentlySelectedCharacterSprite(character2Image, character2Name);
        }
        if (SCscript.currentCharacter.name == character3)
        {
            BrightenCurrentlySelectedCharacterSprite(character3Image, character3Name);
        }
    }

    //Updates and shows current character names and pictures
    public void ShowCurrentCharacters()
    {
        //Update which characters are currently being used
        character1 = SCscript.character1.name;
        character2 = SCscript.character2.name;
        character3 = SCscript.character3.name;

        //Call the ShowCurrentCharacterNames() and ShowCurrentCharacterSprite() functions
        ShowCurrentCharacterNames();
        ShowCurrentCharacterSprite(character1, character1Image); //Character1 Sprite
        ShowCurrentCharacterSprite(character2, character2Image); //Character2 Sprite
        ShowCurrentCharacterSprite(character3, character3Image); //Character3 Sprite
    }

    //Shows names of current characters
    void ShowCurrentCharacterNames()
    {
        character1Name.text = character1;
        character2Name.text = character2;
        character3Name.text = character3;
    }

    //Shows sprites of current characters
    void ShowCurrentCharacterSprite(string character, Image characterImage)
    {
        switch (character)
        {
            case "Warrior":
                characterImage.sprite = Warrior;
                break;
            case "Guardian":
                characterImage.sprite = Guardian;
                break;
            case "Archer":
                characterImage.sprite = Archer;
                break;
            default:
                Debug.Log("Character not found");
                break;
        }
    }

    //Brightens the sprite of the currently active character and dims all others.
    void BrightenCurrentlySelectedCharacterSprite(Image characterImage, TextMeshProUGUI characterName)
    {
        character1Image.color = new Color32(255, 255, 255, 150);
        character2Image.color = new Color32(255, 255, 255, 150);
        character3Image.color = new Color32(255, 255, 255, 150);
        character1Name.color = new Color32(0, 0, 0, 150);
        character2Name.color = new Color32(0, 0, 0, 150);
        character3Name.color = new Color32(0, 0, 0, 150);

        characterImage.color = new Color32(255, 255, 255, 255);
        characterName.color = new Color32(0, 0, 0, 255);
    }
}
