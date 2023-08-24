using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public enum UnitAlignment { lawful, neutral, chaotic}

    public static int[] LawfulBuff = { 0, 25, 25, 0, -25, -25 };
    public static int[] NeutralBuff = { 0, 0, 0, 0, 0, 0 };
    public static int[] ChaoticBuff = { 0, -25, -25, 0, 25, 25 };

    public Battle battle { get; set; }
    public Player Player { get; set; }

    public SpriteRenderer circle;

    public bool isHero { get; set; }

    public string UnitTypeName;
    public UnitAlignment alignment;

    public Unit levelUpUnit;

    public int Cost;

    public int MaxHP;
    public int CurrentHP { get; set; }
    public int MaxMovement;
    public int currentMovement;
    public int CurrentMovement {
        get => currentMovement;
        set
        {
            if (value == MaxMovement)
                circle.color = Player.color;
            else if (value == 0)
                circle.color = Color.red;
            else
                circle.color = Color.yellow;

            currentMovement = value;
        } 
    }

    public int MPCastle;
    public int MPFlat;
    public int MPForest;
    public int MPHills;
    public int MPVillage;
    public int MPWater;

    public bool CanAttack { get; set; }

    public Dictionary<Tile.TerrainType, int> movementCost = new Dictionary<Tile.TerrainType, int>();
    public int MaxXP;
    private int currentXP;
    public int CurrentXP {
        get => currentXP;
        set
        {
            currentXP = value;
            if (currentXP >= MaxXP)
            {
                currentXP -= MaxXP;
                levelUp();
            }
        }
    }

    public int Level;

    public Dictionary<Tile.TerrainType, int> defence = new Dictionary<Tile.TerrainType, int>();
    public int Defence { get; set; }

    public int DefenceCastle;
    public int DefenceFlat;
    public int DefenceForest;
    public int DefenceHills;
    public int DefenceVillage;
    public int DefenceWater;
    public int Resistence { get; set; }

    public List<Attack> Attacks = new List<Attack>();

    public string Attack1Name;
    public int Attack1Damage;
    public int Attack1Hits;
    public Attack.AttackForm Attack1AttackForm;

    public string Attack2Name;
    public int Attack2Damage;
    public int Attack2Hits;
    public Attack.AttackForm Attack2AttackForm;

    // Reduces currentHP by damage done. Returns true if the Unit died, if it is alive return false.
    private bool takeDamage(int damage)
    {
        
        CurrentHP -= damage;
        if (CurrentHP <= 0)
            return true;
        else
            return false;
    }

    // Calculates how much damage the unit takes based on its defence and resistence
    public bool Attacked(Attack attack)
    {
        int random = Random.Range(1, 100);

        if (random > Defence) // is hit
        {
            return takeDamage(attack.Damage * (100 - Resistence) / 100);
        }
        else
            return false;
    }

    private void levelUp()
    {
        if (levelUpUnit == null)
        {
            MaxHP += 4;
            CurrentHP = MaxHP;
        }
        else
        {
            battle.CreateLevelUpUnit(this, levelUpUnit);
        }
    }

    public void Heal(int amount)
    {
        CurrentHP += amount;
        if (CurrentHP > MaxHP)
            CurrentHP = MaxHP;
    }

    public void setDefense(Tile.TerrainType terrain)
    {
        Defence = defence[terrain];
    }

    public void setup(bool isAvaliable, bool isHero)
    {
        this.isHero = isHero;

        movementCost.Add(Tile.TerrainType.castle, MPCastle);
        movementCost.Add(Tile.TerrainType.flat, MPFlat);
        movementCost.Add(Tile.TerrainType.forest, MPForest);
        movementCost.Add(Tile.TerrainType.hills, MPHills);
        movementCost.Add(Tile.TerrainType.village, MPVillage);
        movementCost.Add(Tile.TerrainType.water, MPWater);

        defence.Add(Tile.TerrainType.castle, DefenceCastle);
        defence.Add(Tile.TerrainType.flat, DefenceFlat);
        defence.Add(Tile.TerrainType.forest, DefenceForest);
        defence.Add(Tile.TerrainType.hills, DefenceHills);
        defence.Add(Tile.TerrainType.village, DefenceVillage);
        defence.Add(Tile.TerrainType.water, DefenceWater);

        if (Attack1Name != "")
        {
            Attacks.Add(new Attack(Attack1Name, Attack1Damage, Attack1Hits, Attack1AttackForm));
        }

        if (Attack2Name != "")
        {
            Attacks.Add(new Attack(Attack2Name, Attack2Damage, Attack2Hits, Attack2AttackForm));
        }
        //

        circle = gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        CurrentHP = MaxHP;
        if (isAvaliable)
        {
            CurrentMovement = MaxMovement;
            CanAttack = true;
        }
        else
        {
            CurrentMovement = 0;
            CanAttack = false;
        }
    }

    public void SetNightDayBuff(int dayNightPhase)
    {
        foreach (Attack attack in Attacks)
        {
            attack.SetDayNightBuff(dayNightPhase, alignment);
        }
    }
}
