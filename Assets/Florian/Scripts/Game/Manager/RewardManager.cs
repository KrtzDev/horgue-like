using System.Collections.Generic;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
	[field: SerializeField]
	public List<Reward> drawnRewards = new List<Reward>();

	[SerializeReference]
	private List<WeaponPart> _WeaponPartRewards = new List<WeaponPart>();


	public Reward GetRandomReward()
	{
		Reward drawnReward = new Reward(_WeaponPartRewards[Random.Range(0, _WeaponPartRewards.Count - 1)]);
		drawnRewards.Add(drawnReward);
		return drawnReward;
	}
}
