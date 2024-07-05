using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour, IDataPersistance
{
    [SerializeField]
    private List<Door> doors;

    public enum BaseUpgrade
    {
        None,
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
    private ItemDatabase itemDatabase;


    private void OnEnable()
    {
        foreach (Door door in doors)
        {
            door.canInteract = true;
            door.opened = false;
        }
    }


    public void UpgradeBase(BaseUpgrade upgrade)
    {
        if (baseUpgrades[upgrade] == null)
            baseUpgrades[upgrade] = itemDatabase.GetBaseUpgrade(upgrade, 1);
        else
            baseUpgrades[upgrade] = baseUpgrades[upgrade].nextLevel;

        if (baseUpgrades[upgrade].nextLevel == null)
            Debug.Log("No more possible upgrades for " + upgrade.ToString());

        StartCoroutine(RefreshBaseUpgradeRecipes());
    }
    private IEnumerator RefreshBaseUpgradeRecipes()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        FindAnyObjectByType<PlayerCrafting>().CreateBaseUpgrades();
    }

    public void LoadData(GameData data)
    {
        baseUpgrades = new();
        foreach (BaseUpgrade upgrade in data.baseUpgrades.Keys)
        {
            if (data.baseUpgrades[upgrade] != 0)
                baseUpgrades.Add(upgrade, itemDatabase.GetBaseUpgrade(upgrade, data.baseUpgrades[upgrade]));
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
