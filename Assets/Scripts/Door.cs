using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public bool canInteract;

    private Animator animator;

    void Start()
    {
        canInteract = true;
        animator = GetComponent<Animator>();
    }

    public void Interact()
    {
        Debug.Log("Interaction");
        animator.SetTrigger("OpenDoor");
    }

    public bool CanInteract() { return canInteract; }
}
