using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Character : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _characterRigidbody;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    public PlayerData _playerData;

    public Rigidbody CharacterRigidbody { get => _characterRigidbody; set => _characterRigidbody = value; }
    public Camera Camera { get => _camera; set => _camera = value; }
}
