using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 1f;
    public Rigidbody _rb;

    float x, y;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        MyInput();
        Movement();
    }

    void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
    }

    void Movement()
    {
        _rb.AddForce(new Vector3(x, 0, y) * speed, ForceMode.Force);
    }
}
