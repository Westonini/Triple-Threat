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

    [HideInInspector] public static GameObject currentCharacter;
    private Vector3 currentPosition;

    public static Transform currentPlayerPosition;

    private bool swapOnCooldown;
    private float swapCooldownTimer = 0.25f;

    [Space]
    public GameObject bubbleParticle;
    public GameObject smokeParticles;
    public GameObject spiritsParticles;

    public delegate void FinishCharacterSwap();
    public static event FinishCharacterSwap _finsihedCharacterSwap;

    public delegate void StartCharacterSwap();
    public static event StartCharacterSwap _startedCharacterSwap;

    SceneRestart SR;

    void Awake()
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
    }

    //Subscribe SwapCooldown to _characterSwapped
    private void OnEnable()
    {
        _finsihedCharacterSwap += StartSwapCooldown;
    }
    private void OnDisable()
    {
        _finsihedCharacterSwap -= StartSwapCooldown;
    }

    //Calls the Swap function to swap between characters
    void Update()
    {
        //function that gets player input.
        SwapInput();

        //Gets the current player position for the enemies to track.
        currentPlayerPosition = currentCharacter.transform;
    }

    //If/else statements for checking which character to switch to and if it's possible.
    void SwapInput()
    {
        //If the scene isn't currently restarting, the player isn't currently invincible or dead, and swap currently isn't on cooldown..
        if (!SR.sceneCurrentlyRestarting && !PlayerCharacter.isInvincible && !swapOnCooldown && PlayerHealth.playerHealth > 0)
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

        //Call the _startedCharacterSwap event.
        if (_startedCharacterSwap != null)
        {
            _startedCharacterSwap.Invoke();
        }

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
        FindObjectOfType<AudioManager>().Play("Swap");

        //Call the _finsihedCharacterSwap event.
        if (_finsihedCharacterSwap != null)
        {
            _finsihedCharacterSwap.Invoke();
        }
    }

    void StartSwapCooldown()
    {
        StartCoroutine("SwapCooldown");
    }

    //Cooldown timer for swapping between characters
    IEnumerator SwapCooldown()
    {
        //Put swap on cooldown
        swapOnCooldown = true;
        yield return new WaitForSeconds(swapCooldownTimer);
        swapOnCooldown = false;
    }
}
