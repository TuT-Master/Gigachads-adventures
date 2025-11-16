using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableResource : MonoBehaviour
{
    public List<ItemSO> resources;
    public void Collect()
    {
        Debug.Log("Collecting resource: " + gameObject.name);
    }
}
