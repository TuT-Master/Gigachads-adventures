using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private List<IInteractable> interactablesInRange = new();

    private void Update()
    {
        if (Input.GetButtonDown("Interact") && interactablesInRange.Count > 0)
        {
            interactablesInRange[0].Interact();
            if (!interactablesInRange[0].CanInteract())
                interactablesInRange.Remove(interactablesInRange[0]);
        }
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
        if (interactablesInRange.Contains(interactable))
            interactablesInRange.Remove(interactable);
    }
}
