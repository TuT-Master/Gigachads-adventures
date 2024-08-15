using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IDataPersistance
{
    public Rigidbody _rb;
    public float angleRaw;
    public bool sprint;
    public bool turn;

    public bool canMove = true;

    private PlayerStats playerStats;
    private float x;
    private float y;

    private PlayerCamera playerCamera;
    private float rotateX = 0f;
    
    private bool canSprint;


    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        _rb = GetComponent<Rigidbody>();
        playerCamera = GetComponent<PlayerCamera>();
    }

    void Update()
    {
        if (GetComponent<HUDmanager>().AnyScreenOpen() || playerStats.playerStats == null || !canMove)
            return;
        MyInput();
    }

    void FixedUpdate()
    {
        if (playerStats.playerStats == null)
            return;
        Movement();
    }

    void MyInput()
    {
        // WSAD
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        // Shift
        if(Input.GetKey(KeyCode.LeftShift) && playerStats.playerStats["stamina"] >= 0 && canSprint && (x != 0 || y != 0) && !GetComponent<PlayerFight>().defending)
            sprint = true;
        else
            sprint = false;

        // Check if player can sprint again
        if (playerStats.playerStats["stamina"] / playerStats.playerStats["staminaMax"] < 0.25f && !sprint)
            canSprint = false;
        else
            canSprint = true;

        // Drag
        if (x == 0 && y == 0)
            _rb.drag = 5;
        else
            _rb.drag = 1.5f;

        // Mouse
        rotateX += Input.GetAxisRaw("Mouse X") * (playerCamera.mouseXsensitivity / 100f);
        if(rotateX > 360 || rotateX < -360)
            rotateX = 0;
    }

    void Movement()
    {
        // Rotate player
        transform.rotation = Quaternion.Euler(0, rotateX, 0);
        angleRaw = transform.rotation.y;

        // Movement
        _rb.mass = (playerStats.playerStats["weight"] - 80) / 80;
        if(_rb.mass < 1)
            _rb.mass = 1;
        float moreSpeed = 3f;
        if (sprint)
            moreSpeed = 5f;
        
        _rb.AddRelativeForce(moreSpeed * playerStats.playerStats["speed"] * new Vector3(x / 2, 0, y), ForceMode.Force);
    }

    public void LoadData(GameData data)
    {
        transform.position = data.playerPos;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPos = transform.position;
    }
}
