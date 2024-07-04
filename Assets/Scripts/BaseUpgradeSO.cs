using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseUpgrade", menuName = "Scriptable objects/Base upgrade")]
public class BaseUpgradeSO : ScriptableObject
{
    public int requieredAge;

    public int levelOfUpgrade;

    public PlayerBase.BaseUpgrade baseUpgradeType;

    public List<ScriptableObject> recipeMaterials;
    public List<int> recipeMaterialsAmount;

    public BaseUpgradeSO nextLevel;
}
