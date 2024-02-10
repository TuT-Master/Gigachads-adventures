using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    private float angle;
    public Light light;

    private void Update()
    {
        angle = GetComponent<PlayerMovement>().angleRaw;
        light.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }
}
