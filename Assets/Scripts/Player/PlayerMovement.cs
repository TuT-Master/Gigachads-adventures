using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody _rb;

    [SerializeField]
    private float rotationMin;
    [SerializeField]
    private float rotationMax;
    [SerializeField]
    private GameObject playerGFX;
    private PlayerStats playerStats;
    private float x, y;
    private bool turn;


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
        // WSAD
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        // Mouse
        Vector3 mouse = new();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 6))
            mouse = hit.point;
        Vector2 dir = new(mouse.x - _rb.position.x, mouse.z - _rb.position.z);
        float angle = Mathf.Atan2(dir.normalized.x, dir.normalized.y) * Mathf.Rad2Deg;

        if (angle is >= (-90) and < 0)
        {
            // vlevo nahoøe
            if (angle < -35)
                angle = -35;
            playerGFX.transform.localRotation = Quaternion.Euler(15, 0, 0);
            turn = true;
        }
        else if (angle is <= 90 and >= 0)
        {
            // vpravo nahoøe
            if (angle > 35)
                angle = 35;
            playerGFX.transform.localRotation = Quaternion.Euler(15, 0, 0);
            turn = true;
        }
        else if (angle is > 90 and < 180)
        {
            // vpravo dole
            if (angle < 145)
                angle = 145;
            playerGFX.transform.localRotation = Quaternion.Euler(-15, 0, 0);
            turn = false;
        }
        else
        {
            // vlevo dole
            if (angle > -145)
                angle = -145;
            playerGFX.transform.localRotation = Quaternion.Euler(-15, 0, 0);
            turn = false;
        }

        if(turn)
            for (int i = 0; i < playerGFX.transform.childCount; i++)
            {
                if(playerGFX.transform.GetChild(i).name == "Hair")
                    playerGFX.transform.GetChild(i).localPosition = new(0f, 0.55f, 0.001f);
                else if (playerGFX.transform.GetChild(i).name == "Beard")
                    playerGFX.transform.GetChild(i).localPosition = new(0f, 0.55f, 0.001f);
            }

        _rb.MoveRotation(Quaternion.Euler(0, angle, 0));
    }

    void Movement()
    {
        // Movement
        _rb.mass = playerStats.weight / 40;
        if(_rb.mass < 1)
            _rb.mass = 1;
        _rb.AddForce(new Vector3(x, 0, y) * playerStats.speed, ForceMode.Force);

        // Rotating towards cursor

    }
}
