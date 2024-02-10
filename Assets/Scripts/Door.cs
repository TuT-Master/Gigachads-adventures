using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public int currentRoomId;

    public bool canInteract;
    public bool leadToRoom;

    public int leadToRoomId;

    public string sceneName;



    private Animator animator;


    void Start()
    {
        canInteract = true;
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        if (canInteract)
        {
            animator.SetTrigger("OpenDoor");

            canInteract = false;
        }
        else
        {
            Debug.Log("Locked!");
        }
    }

    public bool CanInteract() { return true; }
}
