using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadEnemyData : MonoBehaviour
{
    [Header("ENEMY DATA")]
    [SerializeField]
    private int _enemyDataAmount;
    [SerializeField]
    private BasicEnemyData _basicEnemyData;
    [SerializeField]
    private BasicEnemyData _rangedRobotData;
    [SerializeField]
    private BasicEnemyData _pasuKanData;

    private string _GoogleURL;

    string[] notes;
    int column = 1;
    int iteration = 0;

    string rowsjson = "";
    string[] lines;
    List<string> eachrow;

    private void Awake()
    {
        for (iteration = 0;  iteration < _enemyDataAmount; iteration++)
        {
            StartCoroutine(ObtainSheetData());
        }
    }

    public IEnumerator ObtainSheetData()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/Enemies?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        UnityWebRequest www = UnityWebRequest.Get(_GoogleURL);
        // get Spreadsheet: https://sheets.googleapis.com/v4/spreadsheets/<ID>/values/<SheetName>?key=<APIKey>
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.timeout > 2)
        {
            Debug.LogError("ERROR: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            var objectInSpreadsheet = JSON.Parse(json);
            foreach (var item in objectInSpreadsheet["values"])
            {
                var itemo = JSON.Parse(item.ToString());
                eachrow = itemo[0].AsStringList;
                foreach (var bro in eachrow)
                {
                    rowsjson += bro + ",";
                }
                rowsjson += "\n";
            }
            lines = rowsjson.Split(new char[] { '\n' });
            notes = lines[column].Split(new char[] { ',' });
            List<string> tempData = new List<string>();
            for (iteration = 0; iteration < notes.Length - 1; iteration++)
            {
                tempData.Add(notes[iteration]);
            }
            ApplySheetData(tempData);
            column++;
        }
    }

    private void ApplySheetData(List<string> tempEnemyData)
    {
        switch (tempEnemyData[0])
        {
            case "Basic":
                _basicEnemyData._maxHealth = int.Parse(tempEnemyData[1]);
                _basicEnemyData._damagePerHit = int.Parse(tempEnemyData[2]);
                _basicEnemyData._attackSpeed = int.Parse(tempEnemyData[3]);
                _basicEnemyData._givenXP = int.Parse(tempEnemyData[4]);
                _basicEnemyData._moveSpeed = int.Parse(tempEnemyData[5]);
                _basicEnemyData._armor = int.Parse(tempEnemyData[6]);
                _basicEnemyData._elementalResistance = int.Parse(tempEnemyData[7]);
                _basicEnemyData._technicalResistance = int.Parse(tempEnemyData[8]);
                Debug.Log("Basic");
                break;
            case "RangedRobot":
                _rangedRobotData._maxHealth = int.Parse(tempEnemyData[1]);
                _rangedRobotData._damagePerHit = int.Parse(tempEnemyData[2]);
                _rangedRobotData._attackSpeed = int.Parse(tempEnemyData[3]);
                _rangedRobotData._givenXP = int.Parse(tempEnemyData[4]);
                _rangedRobotData._moveSpeed = int.Parse(tempEnemyData[5]);
                _rangedRobotData._armor = int.Parse(tempEnemyData[6]);
                _rangedRobotData._elementalResistance = int.Parse(tempEnemyData[7]);
                _rangedRobotData._technicalResistance = int.Parse(tempEnemyData[8]);
                Debug.Log("RangedRobot");
                break;
            case "PasuKan":
                _pasuKanData._maxHealth = int.Parse(tempEnemyData[1]);
                _pasuKanData._damagePerHit = int.Parse(tempEnemyData[2]);
                _pasuKanData._attackSpeed = int.Parse(tempEnemyData[3]);
                _pasuKanData._givenXP = int.Parse(tempEnemyData[4]);
                _pasuKanData._moveSpeed = int.Parse(tempEnemyData[5]);
                _pasuKanData._armor = int.Parse(tempEnemyData[6]);
                _pasuKanData._elementalResistance = int.Parse(tempEnemyData[7]);
                _pasuKanData._technicalResistance = int.Parse(tempEnemyData[8]);
                Debug.Log("PasuKan");
                break;
        }
    }
}
