using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayerData : ReadGoogleSheets
{
    [SerializeField]
    private PlayerData _playerData;

    [System.Serializable]
    public class _PlayerData
    {
        public string name;
        public float movementSpeed;
        public int maxHealth;
    }

    [System.Serializable]
    public class PlayerDataList
    {
        public _PlayerData[] values;
    }

    public PlayerDataList myPlayerDataList = new PlayerDataList();

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/PlayerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override void ApplySheetData(List<string> tempPlayerData)
    {
        if(tempPlayerData[0] == "Player")
        {
            _playerData._movementSpeed = float.Parse(tempPlayerData[1]);
            _playerData._maxHealth = int.Parse(tempPlayerData[2]);
        }
    }

    public override void ReadJSON()
    {
        myPlayerDataList = JsonUtility.FromJson<PlayerDataList>(JSONFile.text);
    }
}
