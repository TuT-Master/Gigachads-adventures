using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable objects/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public string itemName;
    public string description;

    public float damage = 0;
    public float penetration = 0;
    public float armorIgnore = 0;
    public float projectileSpeed = 0;
    public float splashDamage = 0;
    public float splashRadius = 0;
    public float weight = 0;
}
