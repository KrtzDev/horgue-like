using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Ability", menuName = "Abilities")]
public class Ability : ScriptableObject
{
    public string _name;
    public Sprite _icon;
}
