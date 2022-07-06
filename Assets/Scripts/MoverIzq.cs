using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverIzq : MonoBehaviour
{
    public float velocidadTubos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * velocidadTubos * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D ColM)
    {
        if (ColM.gameObject.name == "Bird")
        {
            if (Sonido.NunSoun == 1)
            {
                GetComponent<AudioSource>().Play();
            }
            GameController.Score += 1f;
        }
    }
}
