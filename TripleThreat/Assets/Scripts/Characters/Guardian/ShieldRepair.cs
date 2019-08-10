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

    private delegate void ShieldFunction();
    private event ShieldFunction ShieldFunctions;

    void Start()
    {
        //Start the scene with shieldHealth at 10.
        GuardianAction.shieldHealth = 10;

        shieldRepairTime = 3f;

        //Get the SwapCharacters script from the Main Camera object.
        SCscript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SwapCharacters>();

        //Subscribe some functions to ShieldFunctions
        ShieldFunctions += ShowRepairText;
        ShieldFunctions += ShowShieldDurability;
        ShieldFunctions += ShieldRepairInput;
    }

    void Update()
    {
        //Call the ShieldFunctions Event every frame.
        if (ShieldFunctions != null)
        {
            ShieldFunctions();
        }
    }

    void ShowRepairText()
    {
        //If the shield is broken, show some text saying that it's broken and tell player how to repair it
        if (GuardianAction.shieldHealth <= 0 && !currentlyRepairing)
        {
            shieldRepairedText.SetActive(false);
            shieldBrokenText.SetActive(true);
        }
        else
        {
            shieldBrokenText.SetActive(false);
        }

        //If the repair time was increased, show text saying that its been increased
        if (shieldRepairTime > 3f)
        {
            shieldRepairTimeIncreasedText.SetActive(true);
        }
        else
        {
            shieldRepairTimeIncreasedText.SetActive(false);
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
        if (Input.GetButton("Repair") && GuardianAction.shieldHealth <= 0 && !currentlyRepairing)
        {
            StartCoroutine("Repair");
            shieldRepairingText.SetActive(true);
        }
        //If the player releases the "Repair" key while repairing, stop repairing.
        if (Input.GetButtonUp("Repair") && currentlyRepairing)
        {
            StopCoroutine("Repair");
            currentlyRepairing = false;
            shieldRepairingText.SetActive(false);
        }
    }

    //When initially called set currentlyRepairing to true to prevent this IEnumerator from being called multiple times.
    //After 2.5 seconds set currentlyRepairing to false and repair the shield.
    IEnumerator Repair()
    {
        shieldRepairedText.SetActive(false);
        currentlyRepairing = true;

        yield return new WaitForSeconds(shieldRepairTime);

        currentlyRepairing = false;
        GuardianAction.shieldHealth = 10;
        shieldRepairingText.SetActive(false);
        shieldRepairedText.SetActive(true); //Show text saying "SHIELD REPAIRED" for 2.5 seconds
        shieldRepairTime = 3f;

        yield return new WaitForSeconds(2.5f);

        shieldRepairedText.SetActive(false);
    }
}
