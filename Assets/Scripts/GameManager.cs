using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Battle battle;

    // Start is called before the first frame update
    void Start()
    {
        gridManager.grid = grid;
        battle.grid = grid;
        battle.gridManager = gridManager;

        gridManager.GenerateMap();
        battle.GenerateUnits();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
