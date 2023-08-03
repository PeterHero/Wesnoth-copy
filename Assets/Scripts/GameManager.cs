using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Battle battle;
    [SerializeField] private CanvasManager canvasManager;

    // Start is called before the first frame update
    void Start()
    {
        gridManager.grid = grid;
        gridManager.canvasManager = canvasManager;
        gridManager.battle = battle;
        battle.grid = grid;
        battle.gridManager = gridManager;

        gridManager.GenerateMap();
        battle.PrepareBattle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
