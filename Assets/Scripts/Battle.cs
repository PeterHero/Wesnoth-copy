using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour
{
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
        unitsObjects.Add((GameObject)Instantiate(archerPrefab));
        unitsObjects.Add((GameObject)Instantiate(spearmanPrefab));

        foreach (GameObject unitObject in unitsObjects)
            units.Add(unitObject.GetComponent<Unit>());

        // end of preparation


        Debug.Log(units[0].CurrentHP);
        Debug.Log(units[1].CurrentHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (units.Count == 2)
            Fight(units[0], units[0].Attacks[0], units[1], units[1].Attacks[0]);
    }
}
