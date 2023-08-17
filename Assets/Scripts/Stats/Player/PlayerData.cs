using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    public float movementSpeed = 5f;
    public int maxHealth = 50;
    public float levelMultiplier;
}
