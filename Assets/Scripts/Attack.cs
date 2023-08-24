using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public enum AttackForm { melee, ranged }

    public string name;

    private int damage;
    public int Damage
    {
        get
        {
            return damage * (100 + dayNightBuff) / 100;
        }
        set
        {
            damage = value;
        }
    }
    public int count;

    public AttackForm attackForm;

    private int dayNightBuff = 0;

    public Attack(string name, int damage, int count, AttackForm attackForm)
    {
        this.name = name;
        this.damage = damage;
        this.count = count;
        this.attackForm = attackForm;
    }

    public void SetDayNightBuff(int dayNightPhase, Unit.UnitAlignment alignment)
    {
        switch (alignment)
        {
            case Unit.UnitAlignment.lawful:
                dayNightBuff = Unit.LawfulBuff[dayNightPhase];
                break;
            case Unit.UnitAlignment.neutral:
                dayNightBuff = Unit.NeutralBuff[dayNightPhase];
                break;
            case Unit.UnitAlignment.chaotic:
                dayNightBuff = Unit.ChaoticBuff[dayNightPhase];
                break;
            default:
                break;
        }
    }

}
