using System.Collections;
using UnityEngine;

public class SetPlayerData : ReadGoogleSheets
{
    [SerializeField]
    private PlayerData _playerData;

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/PlayerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override void ApplySheetData()
    {
        _playerData._movementSpeed = float.Parse(_variables[0]);
        _playerData._maxHealth = int.Parse(_variables[1]);
    }
}
