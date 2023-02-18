using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameManagerValues : ReadGoogleSheets
{
    [SerializeField]
    private GameManagerValues _Level01;
    [SerializeField]
    private GameManagerValues _Level02;
    [SerializeField]
    private GameManagerValues _Level03;

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/GameManagerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override void ApplySheetData(List<string> tempGameManagerData)
    {
        switch (tempGameManagerData[0])
        {
            case "Level01":
                _Level01._healthBonus = float.Parse(tempGameManagerData[1]);
                _Level01._damageBonus = float.Parse(tempGameManagerData[2]);
                _Level01._timeToSurvive = int.Parse(tempGameManagerData[6]);
                break;
            case "Level02":
                _Level02._healthBonus = float.Parse(tempGameManagerData[1]);
                _Level02._damageBonus = float.Parse(tempGameManagerData[2]);
                _Level02._timeToSurvive = int.Parse(tempGameManagerData[6]);
                break;
            case "Level03":
                _Level03._healthBonus = float.Parse(tempGameManagerData[1]);
                _Level03._damageBonus = float.Parse(tempGameManagerData[2]);
                _Level03._timeToSurvive = int.Parse(tempGameManagerData[6]);
                break;
        }
    }
}
