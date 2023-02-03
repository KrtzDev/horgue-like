using System.Collections;
using UnityEngine;

public class SetBasicEnemyData: ReadGoogleSheets
{
    [SerializeField]
    private BasicEnemyData _basicEnemyData;

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/BasicEnemyValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override void ApplySheetData()
    {
        Debug.Log("SetBasicEnemyData is active");
        /*
        _basicEnemyData._maxHealth = int.Parse(_variables[0]);
        _basicEnemyData._damagePerHit = int.Parse(_variables[1]);
        _basicEnemyData._attackSpeed = int.Parse(_variables[2]);
        _basicEnemyData._givenXP = int.Parse(_variables[3]);
        _basicEnemyData._moveSpeed = int.Parse(_variables[4]);
        _basicEnemyData._armor = int.Parse(_variables[5]);
        _basicEnemyData._elementalResistance = int.Parse(_variables[6]);
        _basicEnemyData._technicalResistance = int.Parse(_variables[7]);
        */
    }
}
