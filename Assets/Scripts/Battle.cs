using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Battle : MonoBehaviour
{
    public Grid grid;

    public GameObject spearmanPrefab;
    public GameObject archerPrefab;

    List<GameObject> unitsObjects = new List<GameObject>();
    List<Unit> units = new List<Unit>();

    public Battle()
    {
        
    }

    public void Fight(Unit attacker, Attack attackerAttack, Unit defender, Attack defenderAttack=null)
    {
        int attackerCounter = attackerAttack.count;

        int defenderCounter;
        if (defenderAttack == null)
            defenderCounter = 0;
        else
            defenderCounter = defenderAttack.count;

        while(attackerCounter > 0 || defenderCounter > 0)
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

    private void HandleDeathOfUnit(Unit deadUnit, Unit killerUnit=null)
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

    public void SpacePressed()
    {
        //Fight(units[0], units[1]);
    }

    // Start is called before the first frame update
    void Start()
    {      

        unitsObjects.Add((GameObject)Instantiate(archerPrefab, grid.CellToWorld(new Vector3Int(0,4,0)), Quaternion.identity));
        unitsObjects.Add((GameObject)Instantiate(spearmanPrefab, grid.CellToWorld(new Vector3Int(0,1,0)), Quaternion.identity));

        foreach (GameObject unitObject in unitsObjects)
            units.Add(unitObject.GetComponent<Unit>());

        // end of preparation
    }

    // Update is called once per frame
    void Update()
    {
        /*if (units.Count == 2)
            Fight(units[0], units[0].Attacks[0], units[1], units[1].Attacks[0]);*/
    }
}
