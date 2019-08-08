using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Grunt will attack any player character if within close enough range.
public class GruntAction : MonoBehaviour
{
    Grunt gruntScript;

    private Collider hitCollider;

    [HideInInspector] public PlayerCharacter playerHit;

    void Start()
    {
        //Get the collider of the hit area.
        hitCollider = GetComponent<BoxCollider>();

        //Get the grunt's script in the parent object.
        gruntScript = GetComponentInParent<Grunt>();
    }

    //If the grunt touches a player, call the grunt's DealDamage function and pass in the player's PlayerCharacter script.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHit = other.gameObject.GetComponent<PlayerCharacter>();
            gruntScript.DealDamage(playerHit);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerHit = null;
        }
    }
}
