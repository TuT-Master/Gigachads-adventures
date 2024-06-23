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
    public Dictionary<BaseUpgrade, int> baseUpgrades = new()
    {
        {BaseUpgrade.Bed, 0},
        {BaseUpgrade.Chest, 0},
        {BaseUpgrade.Kitchen, 0},
        {BaseUpgrade.AlchemyLab, 0},
        {BaseUpgrade.Smithy, 0},
        {BaseUpgrade.EnchantingTable, 0},
        {BaseUpgrade.Materials, 0},
    };

    private void OnEnable()
    {
        foreach (Door door in doors)
        {
            door.canInteract = true;
            door.opened = false;
        }
    }


    public void LoadData(GameData data)
    {
        // TODO - load crafting levels from baseUpgrades
    }

    public void SaveData(ref GameData data)
    {
        // TODO - save crafting levels from baseUpgrades
    }
}
