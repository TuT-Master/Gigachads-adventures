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


    void Start()
    {
        canInteract = true;
        animator = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        if (canInteract)
        {
            canInteract = false;
            if(baseDoors)
                StartCoroutine(UseDoorsInBase());
            else
                StartCoroutine(UseDoorsInDungeon());
        }
        else
        {
            Debug.Log("Locked!");
        }
    }

    private IEnumerator UseDoorsInDungeon()
    {
        animator.SetTrigger("OpenDoor");
        yield return new WaitForSeconds(1);
        
        room.SetActive(false);
        // Move player to new room and start it
        leadToRoom.GetComponent<DungeonRoom>().StartRoom();
        FindAnyObjectByType<PlayerMovement>().gameObject.transform.position = leadToRoom.GetComponent<DungeonRoom>().doors[(doorId + 2) % 4].transform.position;
        FindAnyObjectByType<PlayerInteract>().RemoveInteractable(this);
    }
    private IEnumerator UseDoorsInBase()
    {
        animator.SetTrigger("OpenDoor");
        yield return new WaitForSeconds(1);

        FindAnyObjectByType<DungeonManager>().LoadScene(sceneName);
        FindAnyObjectByType<PlayerInteract>().RemoveInteractable(this);
    }

    public bool CanInteract() { return true; }
}
