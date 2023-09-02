using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics.Tracing;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public GridManager gridManager;
    [HideInInspector] public BattleManager battleManager;

    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text health;
    [SerializeField] private TMP_Text experience;
    [SerializeField] private TMP_Text movement;
    [SerializeField] private TMP_Text defence;
    [SerializeField] private TMP_Text level;
    [SerializeField] private TMP_Text canAttack;
    [SerializeField] private TMP_Text isHero;
    [SerializeField] private TMP_Text cost;

    [SerializeField] private TMP_Text playerOnTurn;
    [SerializeField] private TMP_Text coins;

    [SerializeField] public TMP_Dropdown recruitableUnits;
    [SerializeField] public GameObject recruitPanel;

    [SerializeField] private TMP_Text attackerName;
    [SerializeField] private TMP_Text attackerHP;
    [SerializeField] private TMP_Text attackerXP;
    [SerializeField] private TMP_Text attacker1Name;
    [SerializeField] private TMP_Text attacker1Chance;
    [SerializeField] private TMP_Text attacker1Dmg;
    [SerializeField] private TMP_Text attacker2Name;
    [SerializeField] private TMP_Text attacker2Chance;
    [SerializeField] private TMP_Text attacker2Dmg;

    [SerializeField] private TMP_Text defenderName;
    [SerializeField] private TMP_Text defenderHP;
    [SerializeField] private TMP_Text defenderXP;
    [SerializeField] private TMP_Text defender1Name;
    [SerializeField] private TMP_Text defender1Chance;
    [SerializeField] private TMP_Text defender1Dmg;
    [SerializeField] private TMP_Text defender2Name;
    [SerializeField] private TMP_Text defender2Chance;
    [SerializeField] private TMP_Text defender2Dmg;

    [SerializeField] public TMP_Dropdown unitsAttacks;
    [SerializeField] public GameObject battlePanel;
    
    [SerializeField] public Image dayNightPanel;
    [SerializeField] private TMP_Text dayNightText;

    [SerializeField] public GameObject gameOverPanel;
    [SerializeField] private TMP_Text gameOver;


    public string Type { set { type.text = value; }}
    public string Health { set { health.text = value; }}
    public string Experience { set { experience.text = value; }}
    public string Movement { set { movement.text = value; }}
    public string Defence { set { defence.text = value; }}
    public string Level { set { level.text = value; }}
    public string CanAttack { set { canAttack.text = value; } }
    public string IsHero { set { isHero.text = value; } }
    public string Cost { set { cost.text = value; } }

    public string PlayerOnTurn { set { playerOnTurn.text = value; } }
    public string Coins { set { coins.text = value; } }

    public string AttackerName { set { attackerName.text = value; } }
    public string AttackerHP { set { attackerHP.text = value; } }
    public string AttackerXP { set { attackerXP.text = value; } }
    public string Attacker1Name { set { attacker1Name.text = value; } }
    public string Attacker1Chance { set { attacker1Chance.text = value; } }
    public string Attacker1Damage { set { attacker1Dmg.text = value; } }
    public string Attacker2Name { set { attacker2Name.text = value; } }
    public string Attacker2Chance { set { attacker2Chance.text = value; } }
    public string Attacker2Damage { set { attacker2Dmg.text = value; } }
    public string DefenderName { set { defenderName.text = value; } }
    public string DefenderHP { set { defenderHP.text = value; } }
    public string DefenderXP { set { defenderXP.text = value; } }
    public string Defender1Name { set { defender1Name.text = value; } }
    public string Defender1Chance { set { defender1Chance.text = value; } }
    public string Defender1Damage { set { defender1Dmg.text = value; } }
    public string Defender2Name { set { defender2Name.text = value; } }
    public string Defender2Chance { set { defender2Chance.text = value; } }
    public string Defender2Damage { set { defender2Dmg.text = value; } }
    public string DayNightText { set { dayNightText.text = value; } }
    public string GameOver { set { gameOver.text = value; } }

    public void DisplayUnitStats(Unit unit, bool displayCost = false)
    {
        Type = unit.UnitTypeName;
        Health = $"HP {unit.CurrentHP}/{unit.MaxHP}";
        Experience = $"XP {unit.CurrentXP}/{unit.MaxXP}";
        Movement = $"MP {unit.CurrentMovement}/{unit.MaxMovement}"; ;
        Defence = $"def {unit.Defence} %";
        Level = $"lvl {unit.Level}";
        CanAttack = $"Can attack? {(unit.CanAttack ? "yes" : "no")}";
        IsHero = (unit.isHero) ? "Hero" : "";
        Cost = (displayCost) ? $"Cost ${unit.Cost}" : "";
    }

    public void DisplayPlayerStats(Player playerOnTurn)
    {
        PlayerOnTurn = $"On turn: {playerOnTurn.playerName}";
        Coins = $"$ {playerOnTurn.coins}";


        List<string> tmpUnits = new List<string>();
        foreach (Unit unit in playerOnTurn.recruitableUnits)
        {
            tmpUnits.Add(unit.UnitTypeName);
        }
        recruitableUnits.ClearOptions();
        recruitableUnits.AddOptions(tmpUnits);
    }

    public void RecruitValueChanged(int value, Player player)
    {
        DisplayUnitStats(player.recruitableUnits[value], true);
    }

    public void CloseRecruit()
    {
        recruitPanel.SetActive(false);
        gridManager.ActionsDisabled = false;
    }

    public void CloseBattle()
    {
        battlePanel.SetActive(false);
        gridManager.ActionsDisabled = false;
    }

    public void DisplayFightStats(Unit attacker, Unit defender)
    {
        AttackerName = attacker.UnitTypeName;
        AttackerHP = $"{attacker.CurrentHP}/{attacker.MaxHP} HP";
        AttackerXP = $"{attacker.CurrentXP}/{attacker.MaxXP} XP";
        Attacker1Name = attacker.Attack1Name;
        Attacker1Chance = $"{defender.Defence} %";
        Attacker1Damage = $"{attacker.Attacks[0].Damage} x {attacker.Attack1Hits}";
        
        if (attacker.Attacks.Count >= 2)
        {
            Attacker2Name = attacker.Attack2Name;
            Attacker2Chance = $"{defender.Defence} %";
            Attacker2Damage = $"{attacker.Attacks[1].Damage} x {attacker.Attack2Hits}";
        }
        else
        {
            Attacker2Name = "";
            Attacker2Chance = "";
            Attacker2Damage = "";
        }

        DefenderName = defender.UnitTypeName;
        DefenderHP = $"{defender.CurrentHP}/{defender.MaxHP} HP";
        DefenderXP = $"{defender.CurrentXP}/{defender.MaxXP} XP";

        List<Attack> responseAttacks = battleManager.GetBestResponseAttacks(attacker.Attacks, defender.Attacks);

        if (responseAttacks[0] != null)
        {
            Defender1Name = responseAttacks[0].name;
            Defender1Chance = $"{attacker.Defence} %";
            Defender1Damage = $"{responseAttacks[0].Damage}x{responseAttacks[0].count}";
        }
        else
        {
            Defender1Name = "";
            Defender1Chance = "";
            Defender1Damage = "";
        }

        if (attacker.Attack2Name != "" && responseAttacks[1] != null)
        {
            Defender2Name = responseAttacks[1].name;
            Defender2Chance = $"{attacker.Defence} %";
            Defender2Damage = $"{responseAttacks[1].Damage}x{responseAttacks[1].count}";
        }
        else
        {
            Defender2Name = "";
            Defender2Chance = "";
            Defender2Damage = "";
        }

        List<string> tmpAttacks = new List<string>();
        foreach (Attack attack in attacker.Attacks)
        {
            tmpAttacks.Add(attack.name);
        }
        unitsAttacks.ClearOptions();
        unitsAttacks.AddOptions(tmpAttacks);
    }
}
