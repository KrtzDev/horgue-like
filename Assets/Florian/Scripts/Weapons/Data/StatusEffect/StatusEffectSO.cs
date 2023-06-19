using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new StatusEffect", menuName = "ModularWeapon/Data/StatusEffect")]
[Serializable]
public class StatusEffectSO : ScriptableObject
{
	[Header("General")]
	public float triggerChance;
	public float effectDuration;
	public FloatRange tickRate;

	[Header("Effects")]
	public bool hasInitialExtraDamage;
	[DrawIf(nameof(hasInitialExtraDamage), true)]
	public float initialDamage;
	[DrawIf(nameof(hasInitialExtraDamage), true)]
	public HorgueVFX initialDamageVFX;

	[Space]
	public bool hasDamageOverTime;
	[DrawIf(nameof(hasDamageOverTime), true)]
	public float dotDamage;
	[DrawIf(nameof(hasDamageOverTime), true)]
	public HorgueVFX dotDamageVFX;

	[Space]
	public bool hasSlow;
	[DrawIf(nameof(hasSlow), true)]
	public float slowAmount;
	[DrawIf(nameof(hasSlow), true)]
	public HorgueVFX slowVFX;


	[Space]
	public bool hasKnockBack;
	[DrawIf(nameof(hasKnockBack), true)]
	public KnockBackType knockBackType;
	[DrawIf(nameof(hasKnockBack), true)]
	public float knockBackStrength;
	[DrawIf(nameof(hasKnockBack), true)]
	public HorgueVFX knockBackVFX;

	[Space]
	public bool hasPierce;
	[DrawIf(nameof(hasPierce), true)]
	public float maxPierceAmount;

	[Header("Propagation")]
	public bool canPropagate;
	[DrawIf(nameof(canPropagate), true)]
	public StatusEffectSO propagatedEffect;
	[DrawIf(nameof(canPropagate), true)]
	public LayerMask layersToPropagateTo;
	[DrawIf(nameof(canPropagate), true)]
	public float propagationChance;
	[DrawIf(nameof(canPropagate), true)]
	public float propagationRange;
	[DrawIf(nameof(canPropagate), true)]
	public int maxPropagateToCount;
	[DrawIf(nameof(canPropagate), true)]
	public int consecutivePropagationCount;
	[DrawIf(nameof(canPropagate), true)]
	public HorgueVFX propagationVFX;
}
