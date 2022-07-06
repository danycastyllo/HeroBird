using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logicaTubos : MonoBehaviour
{
    public float tiempoMax = 1;
    public float tiempoIni = 0;
    public GameObject obstaculoo;
    public float altura;
    // Start is called before the first frame update
    void Start()
    {
        GameObject obstaculoNuevo = Instantiate(obstaculoo);
        obstaculoNuevo.transform.position = new Vector3(0, 0, 0);
        Destroy(obstaculoNuevo, 10);
    }

    // Update is called once per frame
    void Update()
    {
        if(tiempoIni > tiempoMax)
        {
            GameObject obstaculoNuevo = Instantiate(obstaculoo);
            obstaculoNuevo.transform.position = new Vector3(0, Random.Range(-altura, altura), 0);
            Destroy(obstaculoNuevo, 10);
            tiempoIni = 0;
        }
        else
        {
            tiempoIni += Time.deltaTime;
        }
    }
}
