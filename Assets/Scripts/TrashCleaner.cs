using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Removes projectiles after 10 seconds
/// </summary>
public class TrashCleaner : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(Cleaner());
    }

    private IEnumerator Cleaner()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
