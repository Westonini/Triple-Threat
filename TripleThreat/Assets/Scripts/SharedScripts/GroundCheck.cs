using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    Character characterScript;

    void Start()
    {
        characterScript = GetComponentInParent<Character>();
    }

    //Ground Check
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            characterScript.OnGroundTouch();
        }
    }
    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            characterScript.OnGroundLeave();
        }
    }
}
