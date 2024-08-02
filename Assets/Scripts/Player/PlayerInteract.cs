using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private List<IInteractable> interactablesInRange = new();

    private void Update()
    {
        // Show possible interactables near player
        foreach(var interactable in interactablesInRange)
        {
            Debug.Log(interactable.GetTransform().gameObject.name + " at pos: " + interactable.GetTransform().position);
        }

        // Interaction
        if (Input.GetButtonDown("Interact") && interactablesInRange.Count > 0)
        {
            try
            {
                if(interactablesInRange[^interactablesInRange.Count].CanInteract())
                    interactablesInRange[^interactablesInRange.Count].Interact();
                if (!interactablesInRange[^interactablesInRange.Count].CanInteract())
                    RemoveInteractable(interactablesInRange[^interactablesInRange.Count]);
            }
            catch
            {
                interactablesInRange = new();
            }
        }
    }

    public void RemoveInteractable(IInteractable interactable)
    {
        if (interactablesInRange.Contains(interactable))
            interactablesInRange.Remove(interactable);
    }

    private void OnTriggerEnter(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        if (interactable != null && interactable.CanInteract())
            interactablesInRange.Add(interactable);
    }

    private void OnTriggerExit(Collider other)
    {
        var interactable = other.GetComponentInParent<IInteractable>();
        RemoveInteractable(interactable);
    }
}
