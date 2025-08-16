using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    public int doorId;

    [HideInInspector] public bool canInteract;
    public bool baseDoors;

    public GameObject room;
    public GameObject leadToRoom;

    public string sceneName;

    [HideInInspector] public Animator animator;
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
        {
            StartCoroutine(UseDoors());
        }
        else
        {
            Debug.Log("Locked!");
        }
    }

    public Transform GetTransform() { return transform; }

    private IEnumerator UseDoors()
    {
        canInteract = false;
        if (!opened)
        {
            animator.SetTrigger("OpenDoor");
            yield return new WaitForSeconds(1);
        }
        else
        {
            animator.SetTrigger("CloseDoor");
            yield return new WaitForSeconds(1);
        }

        canInteract = true;
        opened = !opened;

        // Play some sound for opening the doors

        if(baseDoors)
        {
            if(sceneName == "Home")
            {
                LeaveThisRoom();
                FindAnyObjectByType<PlayerBase>(FindObjectsInactive.Include).EnterPlayerBase(PlayerBase.Location.Shop);
            }
            else if (sceneName == "Shop")
            {
                LeaveThisRoom();
                FindAnyObjectByType<Shop>(FindObjectsInactive.Include).EnterShop();
            }
            else if (sceneName == "Dungeon")
            {
                LeaveThisRoom();
                FindAnyObjectByType<Dungeon>(FindObjectsInactive.Include).EnterDungeon();
            }
            animator.SetTrigger("CloseDoor");
            opened = false;
        }
    }

    private void LeaveThisRoom()
    {
        room.SetActive(false);
    }

    public bool CanInteract() { return canInteract; }
}
