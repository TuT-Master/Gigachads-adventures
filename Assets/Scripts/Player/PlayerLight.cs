using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    private float angle;
    public Light _light;

    private void Update()
    {
        angle = GetComponent<PlayerMovement>().angleRaw;
        _light.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
    }
}
