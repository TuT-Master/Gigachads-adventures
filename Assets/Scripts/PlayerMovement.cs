using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerStats playerStats;
    public Rigidbody _rb;

    float x, y;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
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
        _rb.mass = playerStats.weight / 40;
        if(_rb.mass < 1)
            _rb.mass = 1;
        _rb.AddForce(new Vector3(x, 0, y) * playerStats.speed, ForceMode.Force);
    }
}
