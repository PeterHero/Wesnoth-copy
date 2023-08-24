using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Battle : MonoBehaviour
{
    public Grid grid { get; set; }
    public GridManager gridManager { get; set; }
    public UIManager UIManager { get; set; }

    public Unit spearmanPrefab;
    public Unit bowmanPrefab;
    public Unit swordsmanPrefab;
    public Unit fighterPrefab;
    public Unit archerPrefab;
    public Unit heroPrefab;

    List<Player> players = new List<Player>();
    public Player playerOnTurn;
    int playerOnTurnIndex;

    int dayNightPhase { get; set; }
    private const int numberOfDayNightPhases = 6;

    public List<Attack> responseAttacks = new List<Attack>();

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
        newUnit.battle = this;
        newUnit.setup(isAvaliable, isHero);
        newUnit.setDefense(gridManager.tiles[new Vector2Int(x, y)].terrain);
        newUnit.SetNightDayBuff(dayNightPhase);
        player.units.Add(newUnit);
    }

    public void CreateLevelUpUnit(Unit oldUnit, Unit leveledUnit)
    {
        var newUnit = Instantiate(leveledUnit, oldUnit.transform.position, Quaternion.identity);
        Vector2Int position = new Vector2Int(grid.WorldToCell(oldUnit.transform.position).x, grid.WorldToCell(oldUnit.transform.position).y);
        gridManager.tiles[position].unit = newUnit;

        newUnit.battle = oldUnit.battle;
        newUnit.Player = oldUnit.Player;
        newUnit.CurrentXP = oldUnit.CurrentXP;
        
        newUnit.setup(oldUnit.CurrentMovement == oldUnit.MaxMovement, oldUnit.isHero);
        newUnit.setDefense(gridManager.tiles[position].terrain);
        newUnit.Player.units.Add(newUnit);

        newUnit.Player.units.Remove(oldUnit);
        Destroy(oldUnit.gameObject);
    }

    public void SummonHero(Unit hero, Player player)
    {
        CreateUnit(hero, player.homeTile.x, player.homeTile.y, player, true, true);

    }

    public void PrepareBattle()
    {
        players.Add(new Player("The Elf Lord", Color.green, new Vector2Int(9,0)));
        players[0].recruitableUnits.Add(fighterPrefab);
        players[0].recruitableUnits.Add(archerPrefab);
        players.Add(new Player("The Human King", Color.blue, new Vector2Int(0, 19)));
        players[1].recruitableUnits.Add(spearmanPrefab);
        players[1].recruitableUnits.Add(bowmanPrefab);

        SummonHero(heroPrefab, players[0]);
        SummonHero(swordsmanPrefab, players[1]);

        playerOnTurnIndex = 0;
        playerOnTurn = players[playerOnTurnIndex];
        UIManager.DisplayPlayerStats(playerOnTurn);

        dayNightPhase = 0;
        UIManager.dayNightPanel.color = Color.yellow;
    }

    public void StartTurn()
    {
        UIManager.DisplayPlayerStats(playerOnTurn);

        foreach (Tile tile in gridManager.tiles.Values)
        {
            if (tile.terrain == Tile.TerrainType.village && tile.unit != null && tile.unit.Player == playerOnTurn)
            {
                tile.unit.Heal(8);
            }
        }
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

        playerOnTurn.coins += playerOnTurn.baseIncome;
        playerOnTurn.coins += playerOnTurn.villageIncome * playerOnTurn.controlledVillages;

        playerOnTurnIndex++;
        if (playerOnTurnIndex == players.Count)
        {
            playerOnTurnIndex = 0;
            dayNightPhase = (dayNightPhase + 1) % numberOfDayNightPhases;
            UIManager.DayNightText = $"{dayNightPhase + 1}/{numberOfDayNightPhases}";
            switch (dayNightPhase)
            {
                case 0:
                    UIManager.dayNightPanel.color = Color.yellow;
                    break;
                case 1:
                    UIManager.dayNightPanel.color = Color.cyan;
                    break;
                case 3:
                    UIManager.dayNightPanel.color = Color.red;
                    break;
                case 4:
                    UIManager.dayNightPanel.color = Color.blue;
                    break;
                
            }
            foreach (Player player in players)
                foreach (Unit unit in player.units)
                    unit.SetNightDayBuff(dayNightPhase);
        }
        playerOnTurn = players[playerOnTurnIndex];

        StartTurn();
    }

    public void recruitValueChanged(int value)
    {
        UIManager.recruitValueChanged(value, playerOnTurn);
    }

    public void OpenRecruit()
    {
        if (gridManager.tiles[playerOnTurn.homeTile].unit != null &&
            gridManager.tiles[playerOnTurn.homeTile].unit.isHero)
        {
            UIManager.recruitPanel.SetActive(true);
            UIManager.displayUnitStats(playerOnTurn.recruitableUnits[0], true);
            gridManager.ActionsDisabled = true;
        }
    }

    public void Recruit()
    {
        Unit unitToRecruit = playerOnTurn.recruitableUnits[UIManager.recruitableUnits.value];

        if (unitToRecruit.Cost > playerOnTurn.coins)
            return;

        List<Vector2Int> adjecentTiles = HexMap.GetAdjencentTiles(playerOnTurn.homeTile);
        foreach (Vector2Int v in adjecentTiles)
        {
            Tile thisTile = gridManager.GetTileAtPosition(v);
            if (thisTile == null)
                continue;

            if (gridManager.tiles[v].unit == null)
            {
                CreateUnit(unitToRecruit, v.x, v.y, playerOnTurn);
                playerOnTurn.coins -= unitToRecruit.Cost;
                UIManager.DisplayPlayerStats(playerOnTurn);
                UIManager.recruitPanel.SetActive(false);
                gridManager.ActionsDisabled = false;
                return;
            }
        }
    }

    public void AttackButton()
    {
        int index = UIManager.unitsAttacks.value;

        Fight(gridManager.tiles[gridManager.ActiveUnit].unit, gridManager.tiles[gridManager.ActiveUnit].unit.Attacks[index], gridManager.tiles[gridManager.ActiveTile].unit, responseAttacks[index]);
        gridManager.tiles[gridManager.ActiveUnit].unit.CurrentMovement = 0;
        gridManager.tiles[gridManager.ActiveUnit].unit.CanAttack = false;
        UIManager.CloseBattle();
    }
}
