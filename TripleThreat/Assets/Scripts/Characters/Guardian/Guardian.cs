﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : PlayerCharacter //Inherits from PlayerCharacter
{
    //Guardian Stats:
    //Can't deal damage but can block all damage; takes no additonal damage when hit; really slow speed (2.5)


    //Guardians's walk speed is set to 2.5 (very slow speed)
    protected override void Movement()
    {
        walkSpeed = 2.5f;
        base.Movement();
    }

    //Guardian's action
    //Hold left click to bring up your shield. As long as you're facing the direction that you're getting hit at with your shield up, you won't take any damage.
    protected override void Action<T>(T component)
    {
        //Set actionScript to equal the component passed in as a parameter, in this case it was the GuardianAction script.
        GuardianAction actionScript = component as GuardianAction;
    }
}
