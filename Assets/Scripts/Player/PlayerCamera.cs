using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public bool canRotateY = true;


    [SerializeField] private Camera _camera;
    private float rotateY = 0f;
    [SerializeField] private float angleMax;
    [SerializeField] private float angleMin;

    public float mouseXsensitivity = 100f;
    public float mouseYsensitivity = 100f;


    void Update()
    {
        if (!canRotateY)
            return;

        MyInput();

        if(rotateY < angleMin)
            rotateY = angleMin;
        else if (rotateY > angleMax)
            rotateY = angleMax;
        _camera.gameObject.transform.localRotation = Quaternion.Euler(-rotateY, 0, 0);
    }

    private void MyInput()
    {
        rotateY += Input.GetAxisRaw("Mouse Y") * (mouseYsensitivity / 100f);
    }
}
