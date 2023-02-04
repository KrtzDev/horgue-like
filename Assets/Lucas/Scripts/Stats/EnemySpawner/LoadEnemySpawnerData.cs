using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadEnemySpawnerData : ReadGoogleSheets
{
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData;

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/EnemySpawnerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override void ApplySheetData(List<string> tempEnemySpawnerData)
    {

        _enemySpawnerData._enemyWavesToSpawn = int.Parse(tempEnemySpawnerData[1]);
        _enemySpawnerData._enemyWaveSize = int.Parse(tempEnemySpawnerData[2]);
        _enemySpawnerData._enemySpawnDelay = int.Parse(tempEnemySpawnerData[3]);
    }
}
