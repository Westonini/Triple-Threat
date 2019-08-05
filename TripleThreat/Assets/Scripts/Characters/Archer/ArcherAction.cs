using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherAction : MonoBehaviour
{
    bool arrowCharged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            //Charge arrow for x seconds
        }
        if (Input.GetButtonUp("Fire1") && arrowCharged)
        {
            //Shoot
        }
    }
}
