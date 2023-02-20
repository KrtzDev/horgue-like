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

    [SerializeField]
    private int _dataEntries;

    public virtual void Awake()
    {
        for (iteration = 0; iteration < _dataEntries; iteration++)
        {
            StartCoroutine(ObtainSheetData());
        }
    }

    public virtual void OnEnable()
    {
        for (iteration = 0; iteration <= _dataEntries; iteration++)
        {
            StartCoroutine(ObtainSheetData());
        }
    }

    private void Update()
    {
        /* Realtime - bessere Methode wohl möglich (call bei Spielstart etc. pp)
         * StartCoroutine(ObtainSheetData());
         */
    }

    public virtual IEnumerator ObtainSheetData()
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
                    rowsjson += bro + ";";
                }
                rowsjson += "\n";
            }
            lines = rowsjson.Split(new char[] { '\n' });
            notes = lines[column].Split(new char[] { ';' });
            List<string> tempData = new List<string>();
            for (iteration = 0; iteration < notes.Length - 1; iteration++)
            {
                tempData.Add(notes[iteration]);
            }
            ApplySheetData(tempData);
            column++;
        }
    }

    public abstract void ApplySheetData(List<string> tempData);

}
