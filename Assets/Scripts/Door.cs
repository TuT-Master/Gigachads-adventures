using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [HideInInspector] public bool canInteract;
    public bool baseDoors;
    [SerializeField] private bool revertToStartStateAfterUse;

    public GameObject room;
    public GameObject leadToRoom;
    public Door leadToDoor;
    [HideInInspector] public VirtualDoor virtualDoor;
    [HideInInspector] public VirtualDoor leadToVirtualDoor;

    public string sceneName;

    [SerializeField] private Animator animator;
    [HideInInspector] public bool opened;

    void Start()
    {
        canInteract = true;
        opened = false;
        animator = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        if (canInteract)
            StartCoroutine(UseDoors());
        else
            Debug.Log("Locked!");
    }

    public Transform GetTransform() { return transform; }

    public void SetVisualState(bool open)
    {
        if(animator == null)
            animator = GetComponentInChildren<Animator>();
        animator.SetTrigger(open ? "OpenDoor" : "CloseDoor");
        opened = open;
    }

    public IEnumerator UseDoors()
    {
        canInteract = false;

        // Play some sound for opening the doors

        if (!opened)
        {
            animator.SetTrigger("OpenDoor");
            yield return new WaitForSeconds(1);
            opened = !opened;
        }
        canInteract = true;

        if (revertToStartStateAfterUse)
        {
            animator.SetTrigger("CloseDoor");
            opened = false;
        }

        if(baseDoors)
        {
            room.SetActive(false);
            if (sceneName == "Home")
            {
                FindAnyObjectByType<PlayerBase>(FindObjectsInactive.Include).gameObject.SetActive(true);

                // If from dungeon -> exit it
                if (virtualDoor != null)
                    FindAnyObjectByType<Dungeon>().ExitDungeon();

                // Enter the player base
                FindAnyObjectByType<PlayerBase>(FindObjectsInactive.Include).EnterPlayerBase();

                // Teleport player to the door position
                FindAnyObjectByType<PlayerStats>().transform.position = leadToDoor.transform.position;
            }
            else if (sceneName == "Shop")
            {
                FindAnyObjectByType<Shop>(FindObjectsInactive.Include).gameObject.SetActive(true);
                FindAnyObjectByType<Shop>(FindObjectsInactive.Include).EnterShop();
            }
            else if (sceneName == "Dungeon")
            {
                FindAnyObjectByType<Dungeon>(FindObjectsInactive.Include).gameObject.SetActive(true);
                FindAnyObjectByType<Dungeon>(FindObjectsInactive.Include).EnterDungeon(FindAnyObjectByType<PlayerStats>().transform);
            }
        }
        else // Dungeon doors
        {
            // Activate the room
            leadToRoom.SetActive(true);

            // Teleport player to the leadToDoor position
            FindAnyObjectByType<PlayerStats>().transform.position = leadToDoor.transform.position;

            // Start next room
            leadToRoom.GetComponent<DungeonRoom>().StartRoom();
        }
    }

    public bool CanInteract() { return canInteract; }
}
