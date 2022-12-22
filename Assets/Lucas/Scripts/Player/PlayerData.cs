using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : ReadGoogleSheets
{

    [field: SerializeField] public int _movementSpeed { get; set; }
    [field: SerializeField] public int _maxHealth { get; set; }

    public override void Start()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/PlayerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Start();
    }

    public override IEnumerator ApplySheetData()
    {
        _movementSpeed = int.Parse(_variables[0]);
        _maxHealth = int.Parse(_variables[1]);

        yield return null;
    }


}
