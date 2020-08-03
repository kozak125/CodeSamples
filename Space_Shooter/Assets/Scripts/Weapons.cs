using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used to compose WeaponsProperties SO and positions of guns to shoot at runtime
/// </summary>

[Serializable]
public class Weapons
{
    public List<GameObject> guns;

    public WeaponProperties weaponsProperties;
}
