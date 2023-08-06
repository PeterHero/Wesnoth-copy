using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GridManager gridManager { get; set; }

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

    public void displayUnitStats(Unit unit, bool displayCost = false)
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
        PlayerOnTurn = playerOnTurn.playerName;
        Coins = $"$ {playerOnTurn.coins}";


        List<string> tmpUnits = new List<string>();
        foreach (Unit unit in playerOnTurn.recruitableUnits)
        {
            tmpUnits.Add(unit.UnitTypeName);
        }
        recruitableUnits.ClearOptions();
        recruitableUnits.AddOptions(tmpUnits);
    }

    public void recruitValueChanged(int value, Player player)
    {
        displayUnitStats(player.recruitableUnits[value], true);
    }

    public void CloseRecruit()
    {
        recruitPanel.SetActive(false);
        gridManager.ActionsDisabled = false;
    }
}
