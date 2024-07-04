using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDataPersistance
{
    [SerializeField]
    private List<Door> doors;

    public enum BaseUpgrade
    {
        Bed,
        Chest,
        Kitchen,
        AlchemyLab,
        Smithy,
        EnchantingTable,
        Materials,
        Upgrade,
    }
    public Dictionary<BaseUpgrade, BaseUpgradeSO> baseUpgrades = new()
    {
        {BaseUpgrade.Bed, null},
        {BaseUpgrade.Chest, null},
        {BaseUpgrade.Kitchen, null},
        {BaseUpgrade.AlchemyLab, null},
        {BaseUpgrade.Smithy, null},
        {BaseUpgrade.EnchantingTable, null},
        {BaseUpgrade.Materials, null},
        {BaseUpgrade.Upgrade, null},
    };

    [SerializeField]
    private List<BaseUpgradeSO> baseUpgradesSO;


    private void OnEnable()
    {
        foreach (Door door in doors)
        {
            door.canInteract = true;
            door.opened = false;
        }
    }


    private BaseUpgradeSO GetBaseUpgradeByTypeAndLevel(BaseUpgrade baseUpgrade, int level)
    {
        foreach (BaseUpgradeSO upgradeSO in baseUpgradesSO)
            if(upgradeSO.baseUpgradeType == baseUpgrade && upgradeSO.levelOfUpgrade == level)
                return upgradeSO;
        return null;
    }

    public void LoadData(GameData data)
    {
        baseUpgrades = new();
        foreach (BaseUpgrade upgrade in data.baseUpgrades.Keys)
        {
            if (data.baseUpgrades[upgrade] != 0)
                baseUpgrades.Add(upgrade, GetBaseUpgradeByTypeAndLevel(upgrade, data.baseUpgrades[upgrade]));
            else
                baseUpgrades.Add(upgrade, null);
        }
    }

    public void SaveData(ref GameData data)
    {
        foreach (BaseUpgrade upgrade in baseUpgrades.Keys)
        {
            if (baseUpgrades[upgrade] != null)
                data.baseUpgrades[upgrade] = baseUpgrades[upgrade].levelOfUpgrade;
            else
                data.baseUpgrades[upgrade] = 0;
        }
    }
}
