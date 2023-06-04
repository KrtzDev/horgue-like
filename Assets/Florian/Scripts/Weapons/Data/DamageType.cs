using UnityEngine;

public abstract class DamageType : ScriptableObject, IApplyEffects
{
    public abstract void ApplyEffect(Enemy enemy);
}
