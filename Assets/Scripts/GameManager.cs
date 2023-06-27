using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    // Start is called before the first frame update
    void Start()
    {
        gridManager.GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
