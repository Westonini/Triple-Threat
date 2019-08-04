using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerCharacter //Inherits from PlayerCharacter
{
    //Archer Stats:
    //Long Ranged Decent Damage; takes 2 additional damage when hit; slow speed (4)


    //Archer's walk speed is set to 4 (slow speed)
    protected override void Movement()
    {
        walkSpeed = 4;
        base.Movement();
    }

    //Archer takes 2 additional damage when hurt.
    protected override void TakeDamage(int damageReceived)
    {
        base.TakeDamage(damageReceived + 2);
    }

    //Archer's action
    //Hold left click to charge an arrow, once fully charged let go of left click to fire.
    protected override void Action<T>(T component)
    {
        //Set actionScript to equal the component passed in as a parameter, in this case it was the ArcherAction script.
        ArcherAction actionScript = component as ArcherAction;
    }
}
