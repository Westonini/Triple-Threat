using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grunt : EnemyCharacter
{
    protected override void Movement()
    {
        walkSpeed = 4;
        base.Movement();
    }

    //Grunt's action
    //Grunt will hit and knockback the player if it gets close enough.
    protected override void Action<T>(T component)
    {
        //Set actionScript to equal the component passed in as a parameter, in this case it was the GruntAction script.
        GruntAction actionScript = component as GruntAction;
    }
}
