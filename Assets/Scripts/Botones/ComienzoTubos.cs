using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComienzoTubos : MonoBehaviour
{
    public GameObject sombra;
    public GameObject generadorTubos;
    public float Sped;
    public static bool OnPerilla = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OnPerilla == true) {
            transform.Rotate(0, 0, 1 * Sped * Time.deltaTime);
            sombra.transform.Rotate(0, 0, 1 * Sped * Time.deltaTime);
        }
    }

    void OnMouseDown()
    {
        StartCoroutine(activarflappy());
        OnPerilla = true;
    }

    void OnMouseUp()
    {
    }


    IEnumerator activarflappy()
    {
        yield return new WaitForSeconds(2f);
        Presionar.hacer = "jugar";
        yield return new WaitForSeconds(0.2f);
        Presionar.hacer = "nada";
    }
}
