using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableRock : MonoBehaviour
{
    [SerializeField] private GameObject[] mineableStages;
    private List<List<MineableResource>> resourcesAtStage;
    // Každý MineableResource má 3 HP. Je-li vytìženo, zmizí a hpPerStage se sníží o 3. Bude-li hráè tìžit MineableRock a ne MineableResources,
    // sníží se hpPerStage o 1. Když hpPerStage dosáhne 0, pøejde MineableRock na další stage i pøesto, že nebudou vytìženy všechny MineableResources.
    private int[] hpPerStage;
    private int currentStage = 0;


    private void Start()
    {
        resourcesAtStage = new();
        hpPerStage = new int[mineableStages.Length];
        for (int i = 0; i < mineableStages.Length; i++)
        {
            resourcesAtStage.Add(new());
            foreach (Transform child in mineableStages[i].transform)
                if (child.TryGetComponent(out MineableResource resource))
                    resourcesAtStage[i].Add(resource);

            // Set HP for each stage
            hpPerStage[i] = resourcesAtStage[i].Count * 3; // Each resource has 3 HP

            // Set first stage active, others inactive
            if (i == 0)
                mineableStages[i].SetActive(true);
            else
                mineableStages[i].SetActive(false);
        }
    }

    public void DecreaseHpPerStage(int amount)
    {
        hpPerStage[currentStage] -= amount;
        if (hpPerStage[currentStage] <= 0)
            IncreaseStage();
    }
    private void IncreaseStage()
    {
        if (currentStage < mineableStages.Length - 1)
        {
            mineableStages[currentStage].SetActive(false);
            currentStage++;
            mineableStages[currentStage].SetActive(true);
        }
        else
            Destroy(gameObject);
    }
}
