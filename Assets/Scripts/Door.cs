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


    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public bool opened;

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
            yield return new WaitForSeconds(0.25f);

        opened = true;

        // Play some sound for opening the doors



        
        if(baseDoors)
        {
            if(sceneName == "Home")
                FindAnyObjectByType<VirtualSceneManager>().LoadScene(sceneName, VirtualSceneManager.CurrentScene.Shop);
            else if (sceneName == "Shop")
                FindAnyObjectByType<VirtualSceneManager>().LoadScene(sceneName, VirtualSceneManager.CurrentScene.Home);
            else if (sceneName == "Dungeon")
                FindAnyObjectByType<VirtualSceneManager>().LoadScene(sceneName, VirtualSceneManager.CurrentScene.Home);
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
