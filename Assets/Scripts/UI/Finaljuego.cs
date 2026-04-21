using System.Collections;
using UnityEngine;

public class Finaljuego : MonoBehaviour
{
    public GameObject surprise;

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(0.99f);
        Destroy(surprise);
    }
}