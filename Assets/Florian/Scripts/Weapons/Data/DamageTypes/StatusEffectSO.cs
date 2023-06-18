using UnityEngine;

[CreateAssetMenu(fileName = "new StatusEffect", menuName = "ModularWeapon/Data/StatusEffect")]
public class StatusEffectSO : ScriptableObject
{
	[Header("General")]
	public float triggerChance;
	public float effectDuration;
	public FloatRange tickRate;

	[Header("Effects")]
	public bool hasInitialExtraDamage;
	public float initialDamage;

	[Space]
	public bool hasDamageOverTime;
	public float dotDamage;

	[Space]
	public bool canSlow;
	public float slowAmount;

	[Space]
	public float maxPierceAmount;

	[Header("Propagation")]
	public bool canPropagate;
	public StatusEffectSO propagatedEffect;
	public float propagationChance;
	public float propagationRange;
	public int maxPropagateToCount;
	public int consecutivePropagationCount;
}
