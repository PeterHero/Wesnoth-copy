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

    List<GameObject> unitsObjects = new List<GameObject>();
    List<Unit> units = new List<Unit>();

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
                Debug.Log($"defender: {defender.CurrentHP}");
            }

            if (defenderCounter > 0)
            {
                defenderCounter--;

                if (attacker.Attacked(defenderAttack))
                {
                    HandleDeathOfUnit(attacker, defender);
                    return;
                }
                Debug.Log($"atacker: {attacker.CurrentHP}");
            }
        }

        attacker.CurrentXP += defender.Level;
        defender.CurrentXP += attacker.Level;

    }

    private void HandleDeathOfUnit(Unit deadUnit, Unit killerUnit = null)
    {
        Debug.Log($"{deadUnit} was killed by {killerUnit}");

        unitsObjects.Remove(deadUnit.gameObject);
        units.Remove(deadUnit);
        Destroy(deadUnit.gameObject);

        if (killerUnit != null)
        {
            killerUnit.CurrentXP += deadUnit.Level == 0 ? 4 : deadUnit.Level * 8;
        }

    }

    public void CreateUnit(Unit unit, int x, int y)
    {
        var newUnit = Instantiate(unit, grid.CellToWorld(new Vector3Int(x, y)), Quaternion.identity);
        gridManager.tiles[new Vector2Int(x, y)].unit = unit;
        units.Add(unit);
    }

    public void GenerateUnits()
    {
        CreateUnit(archerPrefab, 1, 1);
        CreateUnit(spearmanPrefab, 2, 3);
    }
}
