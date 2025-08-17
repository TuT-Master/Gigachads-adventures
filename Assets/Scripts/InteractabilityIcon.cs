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
        Vector3 dir = new(playerCamera.transform.position.x - transform.position.x,
                          0,
                          playerCamera.transform.position.z - transform.position.z);

        if (dir.sqrMagnitude < 0.001f) return; // avoid weird cases when dir is almost 0

        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + 180f;
        //angle += dir.normalized.z > 0 ? 180 : 0; // Adjust angle based on direction
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}
