using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public WeaponMeleeSO weaponMelee;
    public WeaponRangedSO weaponRanged;

    [SerializeField]
    private GameObject weaponGO;
    private MeshRenderer weaponMaterial;

    [SerializeField]
    private ItemDatabase itemDatabase;
    [SerializeField]
    private DungeonDatabase dungeonDatabase;


    void Start()
    {
        weaponMaterial = weaponGO.GetComponent<MeshRenderer>();
        if(weaponMelee != null)
        {
            weaponMaterial.material = dungeonDatabase.GetWeaponMaterial(weaponMelee.itemName);
            if (weaponMaterial.material == null)
                Debug.Log("Enemy " + GetComponent<EnemyStats>().enemyName + " coudn't find any weapon with name " + weaponMelee.itemName);
        }
        else if (weaponRanged != null)
        {
            weaponMaterial.material = FindAnyObjectByType<DungeonDatabase>().GetWeaponMaterial(weaponRanged.itemName);
            if (weaponMaterial.material == null)
                Debug.Log("Enemy " + GetComponent<EnemyStats>().enemyName + " coudn't find any weapon with name " + weaponRanged.itemName);
        }
        if (weaponMelee == null && weaponRanged == null)
            weaponMaterial.gameObject.SetActive(false);
    }

    public bool HasWeapon(out Item weapon)
    {
        if (weaponMelee != null)
            weapon = weaponMelee.ToItem();
        else if (weaponRanged != null)
            weapon = weaponRanged.ToItem();
        else
        {
            weapon = null;
            return false;
        }
        return true;
    }
}
