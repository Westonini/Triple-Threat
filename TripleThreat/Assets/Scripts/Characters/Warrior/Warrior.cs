using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerCharacter //Inherits from PlayerCharacter
{
    //Warrior Stats:
    //Close Ranged Good Damage; takes 1 additional damage when hit; average speed (5)


    //Warrior's walk speed is set to 5 (average speed)
    protected override void Movement()
    {
        walkSpeed = 5;
        base.Movement();
    }

    //Warrior takes 1 additional damage when hurt.
    protected override void TakeDamage(int damageReceived)
    {
        base.TakeDamage(damageReceived + 1);
    }

    //Warrior's action
    //Left click to swing your sword and hit all enemies caught in the sweep.
    protected override void Action<T>(T component)
    {
        //Set actionScript to equal the component passed in as a parameter, in this case it was the WarriorAction script.
        WarriorAction actionScript = component as WarriorAction;
    }
}
