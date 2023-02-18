using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadEnemySpawnerData : ReadGoogleSheets
{
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData1;
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData2;
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData3;

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/GameManagerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override void ApplySheetData(List<string> tempEnemySpawnerData)
    {
        switch (tempEnemySpawnerData[0])
        {
            case "Level01":
                    _enemySpawnerData1._enemyWavesToSpawn = int.Parse(tempEnemySpawnerData[3]);
                    _enemySpawnerData1._enemyWaveSize = int.Parse(tempEnemySpawnerData[4]);
                    _enemySpawnerData1._enemySpawnDelay = int.Parse(tempEnemySpawnerData[5]);
                break;
            case "Level02":
                    _enemySpawnerData2._enemyWavesToSpawn = int.Parse(tempEnemySpawnerData[3]);
                    _enemySpawnerData2._enemyWaveSize = int.Parse(tempEnemySpawnerData[4]);
                    _enemySpawnerData2._enemySpawnDelay = int.Parse(tempEnemySpawnerData[5]);
                break;
            case "Level03":
                    _enemySpawnerData3._enemyWavesToSpawn = int.Parse(tempEnemySpawnerData[3]);
                    _enemySpawnerData3._enemyWaveSize = int.Parse(tempEnemySpawnerData[4]);
                    _enemySpawnerData3._enemySpawnDelay = int.Parse(tempEnemySpawnerData[5]);
                break;
        }
    }
}
