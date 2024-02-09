using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Scriptable objects/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public string itemName;
    public string description;

    public int stackSize;

    public Dictionary<string, float> projectileStats;
    [SerializeField] private float damage = 0;
    [SerializeField] private float penetration = 0;
    [SerializeField] private float armorIgnore = 0;
    [SerializeField] private float projectileSpeed = 0;
    [SerializeField] private float splashDamage = 0;
    [SerializeField] private float splashRadius = 0;
    [SerializeField] private float weight = 0;

    public Sprite sprite_inventory;
    public Sprite sprite_equip;
}
