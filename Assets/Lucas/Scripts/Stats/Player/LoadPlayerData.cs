using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerData : ReadGoogleSheets
{
    [SerializeField]
    private PlayerData _playerData;

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/PlayerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override void ApplySheetData(List<string> tempPlayerData)
    {
        _playerData._movementSpeed = float.Parse(tempPlayerData[1]);
        _playerData._maxHealth = int.Parse(tempPlayerData[2]);
    }
}
