using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ReadGoogleSheets : MonoBehaviour
{
    [HideInInspector] public List<string> _variables;
    [HideInInspector] public string _GoogleURL;

    public virtual void Start()
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
        // müsste dann pro Database Spreadsheet (Spieler, Gegner, Waffe, etc.) ein neues Skript oder einen String erstellen
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError || www.timeout > 2)
        {
            Debug.LogError("ERROR: " + www.error);
        }
        else
        {
            string json = www.downloadHandler.text;
            var objectInSpreadsheet = JSON.Parse(json);

            List<string> _currentRow;
            int currentRow = 0; // neglect first Row

            foreach (var item in objectInSpreadsheet["values"])
            {
                var itemObject = JSON.Parse(item.ToString());
                _currentRow = itemObject[0].AsStringList; // currentRow = aktuelle Zeile; currentRow[index] = aktuelle Spalte (startet bei 0)

                if (currentRow == 0)
                {
                    currentRow++;
                    continue;
                }

                for (int j = 0; j < _currentRow.Count; j++)
                {
                    if (j == 1)
                    {
                        _variables.Add(_currentRow[j]);
                    }
                }

                currentRow++;
            }
        }

        StartCoroutine(ApplySheetData());
    }

    public abstract IEnumerator ApplySheetData();

}
