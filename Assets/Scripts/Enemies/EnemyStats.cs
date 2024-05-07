using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyStats : MonoBehaviour, IInteractableEnemy
{
    public string name = "";
    public bool isChampion;

    [SerializeField] private float hp;
    [SerializeField] private float armor;
    [SerializeField] private float evasion;
    [SerializeField] private float defense;

    public bool isStunned;

    private EffectManager effectManager;

    private float color = 255;



    void Start()
    {
        effectManager = FindAnyObjectByType<EffectManager>();
        if(!isChampion)
            name = gameObject.name;
    }

    void Update()
    {
        if (hp <= 0)
            Die();

        if (color < 255)
        {
            color += 3;
            transform.Find("GFX").GetChild(0).GetComponent<MeshRenderer>().material.color = new Color(255, color / 255f, color / 255f);
        }
        else
            transform.Find("GFX").GetChild(0).GetComponent<MeshRenderer>().material.color = Color.white;
    }

    private void Die()
    {
        ItemSpawner itemSpawner = FindAnyObjectByType<ItemSpawner>();
        itemSpawner.dropItem = true;
        FindAnyObjectByType<ItemSpawner>().Interact();
        itemSpawner.dropItem = false;
        FindAnyObjectByType<PlayerInventory>().DropItemOnDaFloor(FindAnyObjectByType<ItemSpawner>().GetMaterial(Random.Range(1, 3)), transform.position, FindAnyObjectByType<Dungeon>().currentRoom.transform);
        Destroy(gameObject);
    }

    public bool CanInteract()
    {
        if (hp <= 0)
            return false;
        else
            return true;
    }

    public void HurtEnemy(float damage, float penetration, float armorIgnore, out float finalDamage)
    {
        finalDamage = 0f;
        float armorTemp = armor;

        if (armorIgnore > 0)
            armorTemp *= armorIgnore;

        if (armorTemp - penetration > 0)
            finalDamage = damage - (armorTemp - penetration);
        else
            finalDamage = damage;

        if (defense > 0)
            finalDamage *= defense / 100;

        if (finalDamage > 0)
        {
            Debug.Log("Hitting enemy and dealing " + finalDamage + " damage to hp!");
            // Spawn blood stain
            effectManager.SpawnStain(transform, EffectManager.StainType.Blood);

            // Play some sound


            // Hit animation
            color = 0f;
            transform.Find("GFX").GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;


            // Decrease hp
            hp -= finalDamage;
        }
        else
        {
            Debug.Log("Not enough power to break through enemy's armor!");
            // Play some sound
        }
    }

    public void StunEnemy(float seconds)
    {
        StartCoroutine(GetComponent<EnemyMovement>().Stun(seconds));
        isStunned = true;
    }
}
