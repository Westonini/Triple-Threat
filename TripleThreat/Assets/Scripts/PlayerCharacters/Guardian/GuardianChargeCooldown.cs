using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A cooldown script for Guardian's charge action that persists even while Guardian is inactive.
public class GuardianChargeCooldown : MonoBehaviour
{
    public void StartChargeCooldown()
    {
        StartCoroutine("ChargeCooldown");
    }

    private IEnumerator ChargeCooldown()
    {
        GuardianAction.chargeOnCooldown = true;
        yield return new WaitForSeconds(3f);
        GuardianAction.chargeOnCooldown = false;
    } 
}
