using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ReadGoogleSheets : MonoBehaviour
{
    public TextMeshProUGUI outPutArea;

    string rowsInJSON = "";
    List<string> currentRow;

    private void Start()
    {
        StartCoroutine(ObtainSheetData());
    }

    private void Update()
    {
        /* Realtime - bessere Methode wohl möglich (call bei Spielstart etc. pp)
         * StartCoroutine(ObtainSheetData());
         */
    }

    IEnumerator ObtainSheetData()
    {
        UnityWebRequest www = UnityWebRequest.Get("https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/PlayerValues?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY"); 
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

            int currentRow = 0; // neglect first Row

            foreach (var item in objectInSpreadsheet["values"])
            {
                var itemObject = JSON.Parse(item.ToString());
                this.currentRow = itemObject[0].AsStringList; // currentRow = aktuelle Zeile; currentRow[index] = aktuelle Spalte (startet bei 0)

                if (currentRow == 0)
                {
                    currentRow++;
                    continue;
                }

                for (int j = 0; j < this.currentRow.Count; j++)
                {
                    rowsInJSON += this.currentRow[j];
                    if(j == 0)
                    {
                        rowsInJSON += ": ";
                    }
                }

                currentRow++;
                rowsInJSON += "\n";
            }

            outPutArea.text = rowsInJSON;
        }
    }
}
