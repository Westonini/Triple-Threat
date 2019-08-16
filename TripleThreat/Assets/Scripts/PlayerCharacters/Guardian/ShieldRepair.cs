using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//This script is for repairing the shield's health.
//The shield can be repaired no matter what character you're using by holding down the Spacebar key.
public class ShieldRepair : MonoBehaviour
{
    bool currentlyRepairing;

    public GameObject shieldBrokenText;
    public GameObject shieldRepairingText;
    public GameObject shieldRepairedText;
    public GameObject shieldRepairTimeIncreasedText;

    public static float shieldRepairTime;

    public GameObject shieldImage;
    public TextMeshProUGUI shieldDurabilityText;
    SwapCharacters SCscript;

    public delegate void ShieldRepaired();
    public static event ShieldRepaired _shieldRepaired;             //Event to be invoked when the shield gets repaired.

    void Start()
    {
        //Start the scene with shieldHealth at 10.
        GuardianAction.shieldHealth = 10;

        shieldRepairTime = 3f;

        //Get the SwapCharacters script from the Main Camera object.
        SCscript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SwapCharacters>();

        ShowShieldDurability();

        //Subscribe some functions to the _shieldHit event, which gets called whenever the Guardian's shield gets hit
        GuardianAction._shieldHit += ShowShieldDurability;
        GuardianAction._shieldHit += ShowRepairText;
    }

    private void OnDisable()
    {
        GuardianAction._shieldHit -= ShowShieldDurability;
        GuardianAction._shieldHit -= ShowRepairText;
    }

    void Update()
    {
        ShieldRepairInput();
    }

    void ShowRepairText()
    {
        //If the shield is broken, show some text saying that it's broken and tell player how to repair it
        if (GuardianAction.shieldHealth <= 0)
        {
            shieldRepairedText.SetActive(false);
            shieldBrokenText.SetActive(true);
        }

        //If the repair time was increased, show text saying that its been increased
        if (shieldRepairTime > 3f)
        {
            shieldRepairTimeIncreasedText.SetActive(true);
        }
    }


    void ShowShieldDurability()
    {
        //If the player has the Guardian character in any of the character slots, show shield durability
        if (SCscript.character1.name == "Guardian" || SCscript.character2.name == "Guardian" || SCscript.character3.name == "Guardian")
        {
            shieldImage.SetActive(true);
            shieldDurabilityText.text = "Shield Durability:" + "\n" + GuardianAction.shieldHealth.ToString() + " / 10";
        }
    }

    void ShieldRepairInput()
    {
        //If the player holds down the "Repair" key, repair the shield after 2.5 seconds
        if (Input.GetButton("Repair") && GuardianAction.shieldHealth <= 0 && !currentlyRepairing && PlayerHealth.playerHealth > 0)
        {
            StartCoroutine("Repair");
            shieldBrokenText.SetActive(false);
            shieldRepairingText.SetActive(true);
        }
        //If the player releases the "Repair" key while repairing, stop repairing. Alternatively, if the player is dead stop the repair.
        if (Input.GetButtonUp("Repair") && currentlyRepairing || PlayerHealth.playerHealth <= 0)
        {
            StopCoroutine("Repair");
            currentlyRepairing = false;
            shieldBrokenText.SetActive(true);
            shieldRepairingText.SetActive(false);
        }
    }

    //When initially called set currentlyRepairing to true to prevent this IEnumerator from being called multiple times.
    //After 2.5 seconds set currentlyRepairing to false and repair the shield.
    IEnumerator Repair()
    {
        shieldRepairedText.SetActive(false);
        currentlyRepairing = true;

        //Shield start repair sound
        FindObjectOfType<AudioManager>().Play("UI_2");

        yield return new WaitForSeconds(shieldRepairTime);

        currentlyRepairing = false;
        GuardianAction.shieldHealth = 10;

        ShowShieldDurability(); //Update the shield's durability text
        shieldRepairingText.SetActive(false); //Stop showing the text that says "REPAIRING"
        shieldRepairTimeIncreasedText.SetActive(false); //Stop showing the text that stays "INCREASED REPAIR TIME"
        shieldRepairedText.SetActive(true); //Show text saying "SHIELD REPAIRED" for 2.5 seconds
        shieldRepairTime = 3f; //Set the repair time back to normal.

        if (_shieldRepaired != null) //Invokes the _shieldRepaired function which has EnableShield() subscribed to it.
            _shieldRepaired();

        //Shield Repair sound
        FindObjectOfType<AudioManager>().Play("ShieldRepair");

        yield return new WaitForSeconds(2.5f);

        shieldRepairedText.SetActive(false);
    }
}
