using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsTracker : Singleton<StatsTracker>
{
    [Header("Level Stats")]
    public float damageDealtLevel;
    public int shotsFiredLevel;
    public int jumpsUsedLevel;
    public int scoreCollectedLevel;
    public int enemiesKilledLevel;

    [Header("Total Stats")]
    public float damageDealtTotal;
    public int shotsFiredTotal;
    public int jumpsUsedTotal;
    public int scoreCollectedTotal;
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
        scoreCollectedTotal = 0;
        enemiesKilledTotal = 0;
    }

    public void ResetLevelStats()
    {
        damageDealtLevel = 0;
        shotsFiredLevel = 0;
        jumpsUsedLevel = 0;
        scoreCollectedLevel = 0;
        enemiesKilledLevel = 0;
    }

    public void AddLevelStatsToTotal()
    {
        damageDealtTotal += damageDealtLevel;
        shotsFiredTotal += shotsFiredLevel;
        jumpsUsedTotal += jumpsUsedLevel;
        scoreCollectedTotal += scoreCollectedLevel;
        enemiesKilledTotal += enemiesKilledLevel;
    }
}
