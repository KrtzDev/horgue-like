using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerData))]
public class SetPlayerData : ReadGoogleSheets
{
    private PlayerData _playerData;

    public override void Start()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/PlayerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Start();
    }

    public override IEnumerator ApplySheetData()
    {
        _playerData._movementSpeed = int.Parse(_variables[0]);
        _playerData._maxHealth = int.Parse(_variables[1]);

        yield return null;
    }


}
