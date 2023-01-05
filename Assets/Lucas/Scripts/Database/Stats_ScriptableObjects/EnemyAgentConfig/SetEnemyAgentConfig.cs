using System.Collections;

using UnityEngine;

public class SetEnemyAgentConfig : ReadGoogleSheets
{
    [SerializeField]
    private EnemyAgentConfig _enemyAgentConfig;
    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/EnemyAgentConfig?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override IEnumerator ApplySheetData()
    {
        _enemyAgentConfig.maxTime = float.Parse(_variables[0]);
        _enemyAgentConfig.maxDistance = float.Parse(_variables[0]);

        yield return null;
    }
}
