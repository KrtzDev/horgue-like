using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTracker : Singleton<StatsTracker>
{
    [Header("Level Stats")]
    public float damageDealtLevel;
    public int shotsFiredLevel;
    public int jumpsUsedLevel;
    public float coinsCollectedLevel;
    public float coinsCollectedEndOfRound;
    public int enemiesKilledLevel;

    [Header("Total Stats")]
    public float damageDealtTotal;
    public int shotsFiredTotal;
    public int jumpsUsedTotal;
    public int coinsCollectedTotal;
    public int enemiesKilledTotal;

    public void ResetAllStats()
    {
        ResetTotalStats();
        ResetTotalStats();
    }

    public void ResetTotalStats()
    {
        damageDealtTotal = 0;
        shotsFiredTotal = 0;
        jumpsUsedTotal = 0;
        coinsCollectedTotal = 0;
        enemiesKilledTotal = 0;
    }

    public void ResetLevelStats()
    {
        damageDealtLevel = 0;
        shotsFiredLevel = 0;
        jumpsUsedLevel = 0;
        coinsCollectedLevel = 0;
        coinsCollectedEndOfRound = 0;
        enemiesKilledLevel = 0;
    }

    public void AddLevelStatsToTotal()
    {
        damageDealtTotal += damageDealtLevel;
        shotsFiredTotal += shotsFiredLevel;
        jumpsUsedTotal += jumpsUsedLevel;
        coinsCollectedTotal += Mathf.RoundToInt(coinsCollectedLevel - 0.5f);
        coinsCollectedTotal += Mathf.RoundToInt(coinsCollectedEndOfRound - 0.5f);
        enemiesKilledTotal += enemiesKilledLevel;
    }
}
