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

    private float shieldRepairTime = 3f;

    public GameObject shieldImage;
    public TextMeshProUGUI shieldDurabilityText;
    SwapCharacters SCscript;

    void Start()
    {
        //Start the scene with shieldHealth at 10.
        GuardianAction.shieldHealth = 10;

        SCscript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SwapCharacters>();
    }

    void Update()
    {
        //If the shield is broken, show some text saying that it's broken and tell player how to repair it
        if (GuardianAction.shieldHealth <= 0 && !currentlyRepairing)
        {
            shieldBrokenText.SetActive(true);
        }
        else
        {
            shieldBrokenText.SetActive(false);
        }


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

        if (SCscript.character1.name == "Guardian" || SCscript.character2.name == "Guardian" || SCscript.character3.name == "Guardian")
        {
            shieldImage.SetActive(true);
            shieldDurabilityText.text = "Shield Durability:" + "\n" + GuardianAction.shieldHealth.ToString() + " / 10";
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

        yield return new WaitForSeconds(2.5f);

        shieldRepairedText.SetActive(false);
    }
}
