using System.Collections;
using UnityEngine;


public class SetEnemySpawnerData : ReadGoogleSheets
{
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData;

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/EnemySpawnerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override IEnumerator ApplySheetData()
    {
        _enemySpawnerData._enemyWavesToSpawn = int.Parse(_variables[0]);
        _enemySpawnerData._enemyWaveSize = int.Parse(_variables[1]);
        _enemySpawnerData._enemySpawnDelay = int.Parse(_variables[2]);

        yield return null;
    }


}
