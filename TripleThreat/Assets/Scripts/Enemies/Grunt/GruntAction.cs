using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntAction : MonoBehaviour
{
    Grunt gruntScript;

    private Collider hitCollider;

    [HideInInspector] public PlayerCharacter playerHit;

    void Start()
    {
        hitCollider = GetComponent<BoxCollider>();

        gruntScript = GetComponentInParent<Grunt>();
    }

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
