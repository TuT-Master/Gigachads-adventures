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

    [SerializeField]
    private float rotationMin;
    [SerializeField]
    private float rotationMax;
    [SerializeField]
    private GameObject playerGFX;
    private PlayerStats playerStats;
    private float x, y;
    private bool canSprint;


    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        _rb = GetComponent<Rigidbody>();
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
        Vector3 mouse = new();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Floor")))
            mouse = hit.point;
        Vector2 dir = new(mouse.x - _rb.position.x, mouse.z - _rb.position.z);
        float angle = Mathf.Atan2(dir.normalized.x, dir.normalized.y) * Mathf.Rad2Deg;
        angleRaw = angle;

        #region Rotating player towards mouse cursor
        if (angle is >= (-90) and < 0)
        {
            // vlevo nahoře
            if (angle < -35)
                angle = -35;
            turn = true;
        }
        else if (angle is <= 90 and >= 0)
        {
            // vpravo nahoře
            if (angle > 35)
                angle = 35;
            turn = true;
        }
        else if (angle is > 90 and < 180)
        {
            // vpravo dole
            if (angle < 145)
                angle = 145;
            turn = false;
        }
        else
        {
            // vlevo dole
            if (angle > -145)
                angle = -145;
            turn = false;
        }
        #endregion

        if (turn)
            playerGFX.transform.localRotation = Quaternion.Euler(5, 0, 0);
        else
            playerGFX.transform.localRotation = Quaternion.Euler(-10, 0, 0);

        _rb.MoveRotation(Quaternion.Euler(0, angle, 0));
    }

    void Movement()
    {
        // Movement
        _rb.mass = (playerStats.playerStats["weight"] - 80) / 80;
        if(_rb.mass < 1)
            _rb.mass = 1;
        float moreSpeed = 3f;
        if (sprint)
            moreSpeed = 5f;
        _rb.AddForce(moreSpeed * playerStats.playerStats["speed"] * new Vector3(x, 0, y), ForceMode.Force);
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
