using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spearman : Unit
{
    public Spearman()
    {
        MaxHP = 36;
        CurrentHP = MaxHP;
        Movement = 5;

        MaxXP = 42;

        Level = 1;

        Defence = 40;
        Resistence = 0;

        Attacks.Add(new Attack("spear", 7, 3, Attack.AttackForm.melee));

    }
}