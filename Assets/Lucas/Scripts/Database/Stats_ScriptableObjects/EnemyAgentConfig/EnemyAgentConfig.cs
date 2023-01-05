using UnityEngine;

[CreateAssetMenu(fileName = "enemyAgentConfig", menuName = "Data/EnemyState Data/Base Data")]
public class EnemyAgentConfig : ScriptableObject
{
    public float maxTime = 1.0f;
    public float maxDistance = 1.0f;
}
