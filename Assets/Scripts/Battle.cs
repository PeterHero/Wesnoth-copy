using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public Grid grid;
    public GridManager gridManager;

    public Unit spearmanPrefab;
    public Unit bowmanPrefab;
    public Unit swordsmanPrefab;
    public Unit fighterPrefab;
    public Unit archerPrefab;
    public Unit heroPrefab;

    List<Player> players = new List<Player>();
    public Player playerOnTurn;
    int playerOnTurnIndex;

    public Battle()
    {

    }

    public void Fight(Unit attacker, Attack attackerAttack, Unit defender, Attack defenderAttack = null)
    {
        int attackerCounter = attackerAttack.count;

        int defenderCounter;
        if (defenderAttack == null)
            defenderCounter = 0;
        else
            defenderCounter = defenderAttack.count;

        while (attackerCounter > 0 || defenderCounter > 0)
        {
            if (attackerCounter > 0)
            {
                attackerCounter--;

                if (defender.Attacked(attackerAttack))
                {
                    HandleDeathOfUnit(defender, attacker);
                    return;
                }
            }

            if (defenderCounter > 0)
            {
                defenderCounter--;

                if (attacker.Attacked(defenderAttack))
                {
                    HandleDeathOfUnit(attacker, defender);
                    return;
                }
            }
        }

        attacker.CurrentXP += defender.Level;
        defender.CurrentXP += attacker.Level;

    }

    private void HandleDeathOfUnit(Unit deadUnit, Unit killerUnit = null)
    {
        Debug.Log($"{deadUnit} was killed by {killerUnit}");

        if (deadUnit.isHero)
        {
            // the game ends
            gridManager.tileColor = Color.grey;
        }

        deadUnit.Player.units.Remove(deadUnit);
        Destroy(deadUnit.gameObject);

        if (killerUnit != null)
        {
            killerUnit.CurrentXP += deadUnit.Level == 0 ? 4 : deadUnit.Level * 8;
        }

    }

    public void CreateUnit(Unit unit, int x, int y, Player player, bool isAvaliable = false, bool isHero = false)
    {
        var newUnit = Instantiate(unit, grid.CellToWorld(new Vector3Int(x, y)), Quaternion.identity);
        gridManager.tiles[new Vector2Int(x, y)].unit = newUnit;
        newUnit.Player = player;
        newUnit.setup(isAvaliable, isHero);
        newUnit.setDefense(gridManager.tiles[new Vector2Int(x, y)].terrain);
        player.units.Add(newUnit);
    }

    public void PrepareBattle()
    {
        players.Add(new Player("The Elf Lord", Color.green));
        players.Add(new Player("The Human King", Color.blue));

        CreateUnit(heroPrefab, 9, 0, players[0], true, true);
        CreateUnit(swordsmanPrefab, 0, 19, players[1], true, true);

        playerOnTurnIndex = 0;
        playerOnTurn = players[playerOnTurnIndex];
    }

    public void StartTurn()
    {
        
    }

    public void EndTurn()
    {
        gridManager.isActiveUnitSet = false;

        foreach (Unit unit in playerOnTurn.units)
        {
            unit.CurrentMovement = unit.MaxMovement;
            unit.CanAttack = true;
            unit.circle.color = playerOnTurn.color;
        }

        playerOnTurnIndex = (playerOnTurnIndex + 1) % players.Count;
        playerOnTurn = players[playerOnTurnIndex];

        StartTurn();
    }
}
