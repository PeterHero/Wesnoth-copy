using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElvishArcher : Unit
{
    public ElvishArcher()
    {
        MaxHP = 29;
        CurrentHP = MaxHP;
        Movement = 6;

        MaxXP = 44;

        Level = 1;

        Defence = 50;
        Resistence = 0;

        Attacks.Add(new Attack("sword", 5, 2, Attack.AttackForm.melee));

    }
}
