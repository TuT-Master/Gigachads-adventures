using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractabilityIcon : MonoBehaviour
{
    public GameObject interactableObj;
    private PlayerCamera playerCamera;


    private void Start() { playerCamera = FindAnyObjectByType<PlayerCamera>(); }
    private void Update()
    {
        Vector3 dir = new(playerCamera.transform.position.x - transform.position.x, 0, playerCamera.transform.position.z - transform.position.z);
        float angle;
        if (dir.normalized.z > 0)
            angle = (Mathf.Atan(dir.normalized.x / dir.normalized.z) * Mathf.Rad2Deg) + 180;
        else
            angle = Mathf.Atan(dir.normalized.x / dir.normalized.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
