using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checks if the character is grounded
public class GroundCheck : MonoBehaviour
{
    Character characterScript;

    void Start()
    {
        //If the ground check is tagged "Player", meaning it's a player's ground check, get the currentCharacter Character script.
        //otherwise if it's not tagged "Player", meaning it's an enemy or NPC, get the Character script from the parent object.
        characterScript = gameObject.tag == "Player" ? characterScript = SwapCharacters.currentCharacter.GetComponent<Character>() : characterScript = GetComponentInParent<Character>();
    }

    //Ground Check
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            characterScript.OnGroundTouch(); //Call OnGroundTouch() from the character script.
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            characterScript.OnGroundLeave(); //Call OnGroundLeave() from the character script.
        }
    }
}
