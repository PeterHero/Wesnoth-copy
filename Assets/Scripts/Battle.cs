using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public Grid grid;
    public GridManager gridManager;

    public Unit spearmanPrefab;
    public Unit archerPrefab;

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

        deadUnit.Player.units.Remove(deadUnit);
        Destroy(deadUnit.gameObject);

        if (killerUnit != null)
        {
            killerUnit.CurrentXP += deadUnit.Level == 0 ? 4 : deadUnit.Level * 8;
        }

    }

    public void CreateUnit(Unit unit, int x, int y, Player player)
    {
        var newUnit = Instantiate(unit, grid.CellToWorld(new Vector3Int(x, y)), Quaternion.identity);
        gridManager.tiles[new Vector2Int(x, y)].unit = newUnit;
        newUnit.setDefense(gridManager.tiles[new Vector2Int(x, y)].terrain);
        newUnit.setup(true);
        newUnit.Player = player;
        newUnit.circle.color = newUnit.Player.color;
        player.units.Add(newUnit);
    }

    public void PrepareBattle()
    {
        players.Add(new Player("The Elf Lord", Color.green));
        players.Add(new Player("The Human King", Color.blue));

        CreateUnit(archerPrefab, 1, 1, players[0]);
        CreateUnit(archerPrefab, 1, 2, players[0]);
        CreateUnit(spearmanPrefab, 2, 3, players[1]);

        playerOnTurnIndex = 0;
        playerOnTurn = players[playerOnTurnIndex];
    }

    public void StartTurn()
    {
        
    }

    public void EndTurn()
    {
        foreach (Unit unit in playerOnTurn.units)
        {
            unit.CurrentMovement = unit.MaxMovement;
            unit.CanAttack = true;
        }

        playerOnTurnIndex = (playerOnTurnIndex + 1) % players.Count;
        playerOnTurn = players[playerOnTurnIndex];

        StartTurn();
    }
}
