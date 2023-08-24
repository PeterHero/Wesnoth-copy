using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grid grid;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private UIManager UIManager;

    void Start()
    {
        gridManager.grid = grid;
        gridManager.UIManager = UIManager;
        gridManager.battleManager = battleManager;
        battleManager.grid = grid;
        battleManager.gridManager = gridManager;
        battleManager.UIManager = UIManager;

        UIManager.gridManager = gridManager;
        UIManager.battleManager = battleManager;

        gridManager.GenerateMap();
        battleManager.PrepareBattle();
    }
}
