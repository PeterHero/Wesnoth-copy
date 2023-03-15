using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public enum AttackForm { melee, ranged }

    public string name;

    public int damage;
    public int count;

    public AttackForm attackForm;

    public Attack(string name, int damage, int count, AttackForm attackForm)
    {
        this.damage = damage;
        this.count = count;
        this.attackForm = attackForm;
    }

}
