using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    void Start() { StartCoroutine(RemoveEffect()); }
    private IEnumerator RemoveEffect()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
