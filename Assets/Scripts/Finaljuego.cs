using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finaljuego : MonoBehaviour
{
    public GameObject sorpresa;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(quitar());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator quitar()
    {
        yield return new WaitForSeconds(0.99f);
        Destroy(sorpresa);
    }
}
