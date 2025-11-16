using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableResource : MonoBehaviour
{
    public List<ItemSO> resources;


    public void Mine()
    {
        Debug.Log("Mining resource: " + gameObject.name);
    }
}
