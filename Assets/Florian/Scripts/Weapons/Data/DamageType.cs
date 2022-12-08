using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageType : ScriptableObject, IDoDamage
{
    public abstract void DoDamage();
}
