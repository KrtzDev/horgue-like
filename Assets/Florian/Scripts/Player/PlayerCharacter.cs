using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _characterRigidbody;
    [SerializeField]
    private Camera _camera;
	[SerializeField]
	private Transform _WeaponSpawnTransform;
    [SerializeField]
    public PlayerData _playerData;

    public Rigidbody CharacterRigidbody { get => _characterRigidbody; set => _characterRigidbody = value; }
    public Camera Camera { get => _camera; set => _camera = value; }
	public Transform WeaponSpawnTransform { get => _WeaponSpawnTransform; set => _WeaponSpawnTransform = value; }
}
