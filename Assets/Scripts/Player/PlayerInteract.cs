using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private List<IInteractable> interactablesInRange = new();

    private readonly float interactablesVisualizationRange = 4f;
    private readonly float interactablesMaxVisualizationRange = 2f;

    private void Update()
    {
        // Show possible interactables near player
        foreach (IInteractable interactable in FindObjectsOfType<MonoBehaviour>().OfType<IInteractable>())
        {
            float distance = Vector3.Distance(interactable.GetTransform().position, transform.position);
            if (distance <= interactablesVisualizationRange)
            {
                float f = 1 - ((distance - interactablesMaxVisualizationRange) / (interactablesVisualizationRange - interactablesMaxVisualizationRange));
                Color color = new(1, 1, 1, f);

            }
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
