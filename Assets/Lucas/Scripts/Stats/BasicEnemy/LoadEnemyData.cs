using System.Collections.Generic;
using UnityEngine;

public class LoadEnemyData : ReadGoogleSheets
{
    [Header("ENEMY DATA")]
    [SerializeField]
    private BasicEnemyData _basicEnemyData;
    [SerializeField]
    private BasicEnemyData _rangedRobotData;
    [SerializeField]
    private BasicEnemyData _pasuKanData;
    [SerializeField]
    private BasicEnemyData _gigaRangedRobotData;

    [System.Serializable]
    public class _EnemyData
    {
        public string name;
        public float maxHealth;
        public float damagePerHit;
        public float attackSpeed;
        public int givenXP;
        public float moveSpeed;
        public float armor;
        public float elementalResistance;
        public float technicalResistance;
    }

    [System.Serializable]
    public class EnemyDataList
    {
        public _EnemyData[] Enemies;
    }

    public EnemyDataList myEnemyDataList = new EnemyDataList();

    public override void Awake()
    {
        _GoogleURL = "https://sheets.googleapis.com/v4/spreadsheets/1dbgvJsZAh6RdSJZYxfwGvvmaSeoRMNVGeIIugai8UIU/values/Enemies?key=AIzaSyD2YFsTjKNGDId31Yus0bkFR5hr9WK9yyY";

        base.Awake();
    }

    public override void ApplySheetData(List<string> tempEnemyData)
    {
        switch (tempEnemyData[0])
        {
            case "Basic":
                _basicEnemyData._maxHealth = (int)(int.Parse(tempEnemyData[1]) * GameManager.Instance._GameManagerValues[GameManager.Instance._currentLevelArray]._healthBonus);
                _basicEnemyData._damagePerHit = (int)(int.Parse(tempEnemyData[2]) * GameManager.Instance._GameManagerValues[GameManager.Instance._currentLevelArray]._damageBonus);
                _basicEnemyData._attackSpeed = int.Parse(tempEnemyData[3]);
                _basicEnemyData._givenXP = int.Parse(tempEnemyData[4]);
                _basicEnemyData._moveSpeed = int.Parse(tempEnemyData[5]);
                _basicEnemyData._armor = int.Parse(tempEnemyData[6]);
                _basicEnemyData._elementalResistance = int.Parse(tempEnemyData[7]);
                _basicEnemyData._technicalResistance = int.Parse(tempEnemyData[8]);
                Debug.Log("Basic");
                break;
            case "RangedRobot":
                _rangedRobotData._maxHealth = (int)((float.Parse(tempEnemyData[1]) * _basicEnemyData._maxHealth));
                _rangedRobotData._damagePerHit = (int)(float.Parse(tempEnemyData[2]) * _basicEnemyData._damagePerHit);
                _rangedRobotData._attackSpeed = float.Parse(tempEnemyData[3]);
                _rangedRobotData._givenXP = int.Parse(tempEnemyData[4]);
                _rangedRobotData._moveSpeed = float.Parse(tempEnemyData[5]);
                _rangedRobotData._armor = int.Parse(tempEnemyData[6]);
                _rangedRobotData._elementalResistance = int.Parse(tempEnemyData[7]);
                _rangedRobotData._technicalResistance = int.Parse(tempEnemyData[8]);
                Debug.Log("RangedRobot");
                break;
            case "PasuKan":
                _pasuKanData._maxHealth = (int)(float.Parse(tempEnemyData[1]) * _basicEnemyData._maxHealth);
                _pasuKanData._damagePerHit = (int)(float.Parse(tempEnemyData[2]) * _basicEnemyData._damagePerHit);
                _pasuKanData._attackSpeed = float.Parse(tempEnemyData[3]);
                _pasuKanData._givenXP = int.Parse(tempEnemyData[4]);
                _pasuKanData._moveSpeed = float.Parse(tempEnemyData[5]);
                _pasuKanData._armor = int.Parse(tempEnemyData[6]);
                _pasuKanData._elementalResistance = int.Parse(tempEnemyData[7]);
                _pasuKanData._technicalResistance = int.Parse(tempEnemyData[8]);
                Debug.Log("PasuKan");
                break;
            case "GIGARangedRobot":
                _gigaRangedRobotData._maxHealth = (int)(float.Parse(tempEnemyData[1]) * _basicEnemyData._maxHealth);
                _gigaRangedRobotData._damagePerHit = (int)(float.Parse(tempEnemyData[2]) * _basicEnemyData._damagePerHit);
                _gigaRangedRobotData._attackSpeed = float.Parse(tempEnemyData[3]);
                _gigaRangedRobotData._givenXP = int.Parse(tempEnemyData[4]);
                _gigaRangedRobotData._moveSpeed = float.Parse(tempEnemyData[5]);
                _gigaRangedRobotData._armor = int.Parse(tempEnemyData[6]);
                _gigaRangedRobotData._elementalResistance = int.Parse(tempEnemyData[7]);
                _gigaRangedRobotData._technicalResistance = int.Parse(tempEnemyData[8]);
                Debug.Log("GIGA RangedRobot");
                break;
        }
    }

    public override void ReadJSON()
    {
        myEnemyDataList = JsonUtility.FromJson<EnemyDataList>(JSONFile.text);
    }
}
