using UnityEngine;

[CreateAssetMenu(fileName = "newGameManagerValues", menuName = "Data/GameManager/Level")]
public class GameManagerValues : ScriptableObject
{
    [Header("General")]
    public float _healthBonus;
    public float _damageBonus;
}
