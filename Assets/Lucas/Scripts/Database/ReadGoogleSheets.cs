using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ReadGoogleSheets : MonoBehaviour
{
    [HideInInspector] public string _GoogleURL;

    string[] notes;
    int column = 1;
    int iteration = 0;

    string rowsjson = "";
    string[] lines;
    List<string> eachrow;

    string testString;

    public virtual void Awake()
    {
        StartCoroutine(ObtainSheetData());
    }

    private void Update()
    {
        /* Realtime - bessere Methode wohl möglich (call bei Spielstart etc. pp)
         * StartCoroutine(ObtainSheetData());
         */
    }

    public IEnumerator ObtainSheetData()
    {
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
            for (iteration = 1; iteration < notes.Length - 1; iteration++)
            {
                // output               
                testString += ", " + notes[iteration];
                Debug.Log(testString);
            }
            column++;

        }

        ApplySheetData();
    }

    public abstract void ApplySheetData();

}
