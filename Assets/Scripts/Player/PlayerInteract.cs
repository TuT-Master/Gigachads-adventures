using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private GameObject interactabilityIcon_prefab;
    private Dictionary<IInteractable, GameObject> interactabilityIcons = new();

    private List<IInteractable> interactablesInRange = new();

    private readonly float interactablesVisualizationRange = 3f;
    private readonly float interactablesMaxVisualizationRange = 1.5f;

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
                if (interactable.GetTransform().TryGetComponent(out ItemOnDaFloor itemOnDaFloor))
                {
                    if (!interactabilityIcons.ContainsKey(interactable))
                    {
                        Vector3 newIconPos = new(interactable.GetTransform().position.x, interactable.GetTransform().position.y, interactable.GetTransform().position.z);
                        GameObject newIcon = Instantiate(interactabilityIcon_prefab, newIconPos, Quaternion.identity);
                        interactabilityIcons.Add(interactable, newIcon);
                    }

                    if (interactablesInRange.Contains(interactable))
                    {
                        interactabilityIcons[interactable].GetComponentInChildren<SpriteRenderer>().color = color;
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().color = color;
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to pick up " + itemOnDaFloor.item.itemName;
                    }
                    else
                    {
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "";
                        interactabilityIcons[interactable].GetComponentInChildren<SpriteRenderer>().color = new(1, 1, 1, 0);
                    }
                }
                else
                {
                    if (interactabilityIcons.ContainsKey(interactable))
                    {
                        interactabilityIcons[interactable].GetComponentInChildren<SpriteRenderer>().color = color;
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().color = color;
                    }
                    else
                    {
                        Vector3 newIconPos = new(interactable.GetTransform().position.x, interactable.GetTransform().position.y, interactable.GetTransform().position.z);
                        GameObject newIcon = Instantiate(interactabilityIcon_prefab, newIconPos, Quaternion.identity);
                        newIcon.GetComponentInChildren<SpriteRenderer>().color = color;
                        newIcon.GetComponentInChildren<TextMesh>().color = color;
                        interactabilityIcons.Add(interactable, newIcon);
                    }

                    if(interactablesInRange.Contains(interactable))
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "Press F to interact";
                    else
                        interactabilityIcons[interactable].GetComponentInChildren<TextMesh>().text = "";
                }
            }
            else if(distance > interactablesVisualizationRange && interactabilityIcons.ContainsKey(interactable))
            {
                Destroy(interactabilityIcons[interactable]);
                interactabilityIcons.Remove(interactable);
            }
        }

        // Interaction
        if (Input.GetButtonDown("Interact") && interactablesInRange.Count > 0)
        {
            try
            {
                if(interactablesInRange[0].CanInteract())
                {
                    interactablesInRange[0].Interact();
                    Destroy(interactabilityIcons[interactablesInRange[0]]);
                    interactabilityIcons.Remove(interactablesInRange[0]);
                }
                if (!interactablesInRange[0].CanInteract())
                    RemoveInteractable(interactablesInRange[0]);
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
