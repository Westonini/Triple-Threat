using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is used to swap between the three currently selected characters
public class SwapCharacters : MonoBehaviour
{
    public GameObject character1;
    public GameObject character2;
    public GameObject character3;

    public GameObject[] cameraList;

    [HideInInspector] public GameObject currentCharacter;
    private Vector3 currentPosition;

    public static Transform currentPlayerPosition;

    [HideInInspector] public bool swapOnCooldown;
    private float swapCooldownTimer = 0.25f;
    private float resetSwapCooldownTimer;

    [Space]
    public GameObject bubbleParticle;
    public GameObject smokeParticles;

    SceneRestart SR;

    void Start()
    {
        //Start the level with character1.
        character1.SetActive(true);
        character2.SetActive(false);
        character3.SetActive(false);

        currentCharacter = character1;
        currentPlayerPosition = currentCharacter.transform;

        //Start the level with character1's camera active.
        cameraList[0].SetActive(true);

        //Get the SceneRestart script from the Main Camera object.
        SR = GetComponent<SceneRestart>();

        //Set resetSwapCooldownTimer to swapCooldownTimer to reset the value later.
        resetSwapCooldownTimer = swapCooldownTimer;
    }

    //Calls the Swap function to swap between characters
    void Update()
    {
        //function that gets player input.
        SwapInput();

        //Gets the current player position for the enemies to track.
        currentPlayerPosition = currentCharacter.transform;

        //Swap cooldown function.
        SwapCooldown();
    }

    //If/else statements for checking which character to switch to and if it's possible.
    void SwapInput()
    {
        //If the scene isn't currently restarting, the player isn't currently invincible, and swap currently isn't on cooldown..
        if (!SR.sceneCurrentlyRestarting && !PlayerCharacter.isInvincible && !swapOnCooldown)
        {
            //If the player presses the "Chraracter1" Key and is not currently character 1, swap to character1.
            if (Input.GetButtonDown("Character1") && currentCharacter != character1)
            {
                Swap(ref character1, cameraList[0]);
            }
            //If the player presses the "Chraracter2" Key and is not currently character 2, swap to character2.
            if (Input.GetButtonDown("Character2") && currentCharacter != character2)
            {
                Swap(ref character2, cameraList[1]);
            }
            //If the player presses the "Chraracter3" Key and is not currently character 3, swap to character3.
            if (Input.GetButtonDown("Character3") && currentCharacter != character3)
            {
                Swap(ref character3, cameraList[2]);
            }
        }
    }

    //Quickly sets all characters inactive before swapping to desired character.
    void Swap(ref GameObject character, GameObject cameraToEnable)
    {
        //Sets the current position as the transform of the currently used character.
        currentPosition = currentCharacter.transform.position;

        //Disable all characters
        character1.SetActive(false);
        character2.SetActive(false);
        character3.SetActive(false);

        //Disable all cameras and re-enable the camera of the desired character
        cameraList[0].SetActive(false);
        cameraList[1].SetActive(false);
        cameraList[2].SetActive(false);
        cameraToEnable.SetActive(true);

        //Enable the desired character and set that character's position to that of the previously used character's. Set current character to the character that was just swapped to.
        character.SetActive(true);
        character.transform.position = currentPosition;
        currentCharacter = character;

        //Instantiate Particles when swapping characters.
        InstantiateParticles.InstantiateParticle(currentCharacter.transform, bubbleParticle, 2f, 0.25f, currentCharacter);
        InstantiateParticles.InstantiateParticle(currentCharacter.transform, smokeParticles, 2f, 0.25f);

        //Play sound

        //Put swap on cooldown
        swapOnCooldown = true;
    }

    //Cooldown timer for swapping between characters
    void SwapCooldown()
    {
        if (swapOnCooldown)
        {
            swapCooldownTimer -= Time.deltaTime;

            if (swapCooldownTimer <= 0)
            {
                swapOnCooldown = false;
                swapCooldownTimer = resetSwapCooldownTimer;
            }
        }
    }
}
