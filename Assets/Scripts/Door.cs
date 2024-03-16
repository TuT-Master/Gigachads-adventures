using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    public int doorId;

    [HideInInspector]
    public bool canInteract;
    public bool baseDoors;

    public GameObject room;
    public GameObject leadToRoom;

    public string sceneName;



    private Animator animator;

    private bool opened;

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

    private IEnumerator UseDoors()
    {
        canInteract = false;
        if (!opened)
            animator.SetTrigger("OpenDoor");
        opened = true;

        // Play some sound for opening the doors



        yield return new WaitForSeconds(1);
        
        if(baseDoors)
        {
            FindAnyObjectByType<DungeonManager>().LoadScene(sceneName);
        }
        else
        {
            room.SetActive(false);
            // Move player to new room and start it
            leadToRoom.GetComponent<DungeonRoom>().StartRoom();
            FindAnyObjectByType<PlayerMovement>().gameObject.transform.position = leadToRoom.GetComponent<DungeonRoom>().doors[(doorId + 2) % 4].transform.position;
        }
    }

    public bool CanInteract() { return canInteract; }
}
