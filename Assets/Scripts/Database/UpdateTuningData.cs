// GDoc Tuning Exporter
// (c) 2017 Simon Oliver / HandCircus / hello@handcircus.com
// https://github.com/handcircus/gdoc_tuning_exporter
// Public domain, do with whatever you like, commercial or not
// This comes with no warranty, use at your own risk!

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Networking;

public class UpdateTuningData : MonoBehaviour
{
    public void Start()
    {
		GetTuningDataGame();
    }

    public class ExportError
	{
		public string status;
		public string error;
	}

	private static string ExportSheetScriptURL = "https://script.google.com/macros/s/AKfycbze8vlsLrVsqLOOiF3C_Pej46VasIxAPmjKVRM6LqcEYSJE4cSxcE6fbA6MNyyu_C8/exec";


	private static List<string[]> s_DataUpdateQueue = new List<string[]>();
	static UnityWebRequest www;

	public GameObject GameDataReader;
	public bool dataRetrieved = false;

	public void GetTuningDataGame()
	{
		Debug.Log(EditorPrefs.GetBool(SettingName));

		if (!EditorPrefs.GetBool(SettingName))
        {
			s_DataUpdateQueue.Clear();
			// ADD ALL SPREADSHEETS YOU WANT DO RETRIEVE CONFIG DATA FROM
			// FIRST PART IS LOCAL FILE NAME TO CACHE (IN ASSETS FOLDER), SECOND IS SPREADSHEET KEY
			s_DataUpdateQueue.Add(new string[] { "GameData.json", "1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU" });

			Debug.Log("Get Data");
			SendNextRequestGame();
		}
		else
        {
			Debug.Log("Game Data set Active");
			GameDataReader.SetActive(true);
		}
	}

	static void SendNextRequestGame()
	{
		if (s_DataUpdateQueue.Count == 0)
		{
			Debug.LogError("Queue empty - add some items!");
			return;
		}
		Debug.Log("Retrieving '" + s_DataUpdateQueue[0][0] + "' from sheet '" + s_DataUpdateQueue[0][1] + "'");
		WWWForm form = new WWWForm();
		form.AddField("key", s_DataUpdateQueue[0][1]); // Add spreadsheet key to post darta			
		www = UnityWebRequest.Post(ExportSheetScriptURL, form);
		www.SendWebRequest();
	}

	private void Update()
	{
		if(!EditorPrefs.GetBool(SettingName))
        {
			if (!dataRetrieved)
			{
				while (!www.isDone)
					return;

				bool error = false;
				string errorMessage = "";
				// General error with transport?
				if (www.result == UnityWebRequest.Result.ConnectionError)
				{
					error = true;
					errorMessage = "Error updating data from sheet '" + s_DataUpdateQueue[0][1] + "' : " + www.error;

					dataRetrieved = true;
					GameDataReader.SetActive(true);
				}
				else
				{
					// Detect error with data
					try
					{
						ExportError jsonError = JsonUtility.FromJson<ExportError>(www.downloadHandler.text);
						if (jsonError != null && jsonError.error != null)
						{
							error = true;
							errorMessage = "Error updating data from sheet '" + jsonError.error + "'";

							dataRetrieved = true;
							GameDataReader.SetActive(true);
						}
					}
					catch
					{
						// This is all good, means data is not an error packet 
					}
				}

				if (error)
				{
					Debug.LogError(errorMessage);
					EditorApplication.update -= Update;

					dataRetrieved = true;
					GameDataReader.SetActive(true);
				}
				else
				{
					Debug.Log("Data successfully received from sheet '" + s_DataUpdateQueue[0][1] + "'");
					string writePath = "Assets/" + s_DataUpdateQueue[0][0];
					System.IO.File.WriteAllText(writePath, www.downloadHandler.text);
					s_DataUpdateQueue.RemoveAt(0);
					if (s_DataUpdateQueue.Count == 0)
					{
						// Queue complete
						dataRetrieved = true;
						GameDataReader.SetActive(true);
						EditorApplication.update -= Update;
						AssetDatabase.Refresh();
						www = null;
					}
					else
					{
						SendNextRequestGame();
					}
				}
			}
		}

	}

	private const string MenuName = "Debug Mode/Offline Mode";
	private const string SettingName = "Active";

	public static bool IsEnabled
    {
		get { return EditorPrefs.GetBool(SettingName, true);  }
		set { EditorPrefs.SetBool(SettingName, value); }
    }

	[MenuItem(MenuName)]
	private static void ToggleAction()
    {
		IsEnabled = !IsEnabled;
		Debug.Log("Debug Mode is now: " + IsEnabled);
    }

	[MenuItem(MenuName, true)]
	private static bool ToggleActionValidate()
    {
		Menu.SetChecked(MenuName, IsEnabled);
		return true;
    }


	/*

	[MenuItem("GetJSON/Get Game Data")]
	public static void GetTuningDataEditor()
	{
		s_DataUpdateQueue.Clear();
		// ADD ALL SPREADSHEETS YOU WANT DO RETRIEVE CONFIG DATA FROM
		// FIRST PART IS LOCAL FILE NAME TO CACHE (IN ASSETS FOLDER), SECOND IS SPREADSHEET KEY
		s_DataUpdateQueue.Add(new string[] { "GameData.json", "1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU" });

		SendNextRequestEditor();
		if (www != null) EditorApplication.update += EditorUpdate;
	}

    static void SendNextRequestEditor()
	{
		if (s_DataUpdateQueue.Count == 0)
		{
			Debug.LogError("Queue empty - add some items!");
			return;
		}
		Debug.Log("Retrieving '" + s_DataUpdateQueue[0][0] + "' from sheet '" + s_DataUpdateQueue[0][1] + "'");
		WWWForm form = new WWWForm();
		form.AddField("key", s_DataUpdateQueue[0][1]); // Add spreadsheet key to post darta			
		www = UnityWebRequest.Post(ExportSheetScriptURL, form);
		www.SendWebRequest();
	}

	static void EditorUpdate()
	{
		while (!www.isDone)
			return;

		bool error = false;
		string errorMessage = "";
		// General error with transport?
		if (www.result == UnityWebRequest.Result.ConnectionError)
		{
			error = true;
			errorMessage = "Error updating data from sheet '" + s_DataUpdateQueue[0][1] + "' : " + www.error;
		}
		else
		{
			// Detect error with data
			try
			{
				ExportError jsonError = JsonUtility.FromJson<ExportError>(www.downloadHandler.text);
				if (jsonError != null && jsonError.error != null)
				{
					error = true;
					errorMessage = "Error updating data from sheet '" + jsonError.error + "'";
				}
			}
			catch
			{
				// This is all good, means data is not an error packet 
			}
		}

		if (error)
		{
			Debug.LogError(errorMessage);
			EditorApplication.update -= EditorUpdate;
		}
		else
		{
			Debug.Log("Data successfully received from sheet '" + s_DataUpdateQueue[0][1] + "'");
			string writePath = "Assets/" + s_DataUpdateQueue[0][0];
			System.IO.File.WriteAllText(writePath, www.downloadHandler.text);
			s_DataUpdateQueue.RemoveAt(0);
			if (s_DataUpdateQueue.Count == 0)
			{
				// Queue complete
				EditorApplication.update -= EditorUpdate;
				AssetDatabase.Refresh();
				www = null;
			}
			else
			{
				SendNextRequestEditor();
			}
		}
	}

	*/
}