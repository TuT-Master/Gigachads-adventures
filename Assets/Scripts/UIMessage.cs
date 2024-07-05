using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMessage : MonoBehaviour
{
    void Start() { StartCoroutine(RemoveEffect()); }
    private IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    public void SetUpMessage(string message) { GetComponentInChildren<TextMeshProUGUI>().text = message; }
}
