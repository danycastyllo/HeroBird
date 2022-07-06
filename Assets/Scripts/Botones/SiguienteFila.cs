using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiguienteFila : MonoBehaviour {

	public int Num;

	public GameObject[] Secc;
	public GameObject[] Seccion;

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update () {
        print(SaveBird.SeccNum);
    }
	void OnMouseDown(){
		SaveBird.SeccNum = Num;
        // desactivando todas las filas
        Secc[0].GetComponent<CircleCollider2D>().enabled = true;
        Secc[1].GetComponent<CircleCollider2D>().enabled = true;
        Secc[2].GetComponent<CircleCollider2D>().enabled = true;
        Secc[3].GetComponent<CircleCollider2D>().enabled = true;
        Secc[4].GetComponent<CircleCollider2D>().enabled = true;
        Secc[0].transform.localScale = new Vector2(0.6f, 0.6f);
        Secc[1].transform.localScale = new Vector2(0.6f, 0.6f);
        Secc[2].transform.localScale = new Vector2(0.6f, 0.6f);
        Secc[3].transform.localScale = new Vector2(0.6f, 0.6f);
        Secc[4].transform.localScale = new Vector2(0.6f, 0.6f);
        Seccion[0].SetActive(false);
        Seccion[1].SetActive(false);
        Seccion[2].SetActive(false);
        Seccion[3].SetActive(false);
        Seccion[4].SetActive(false);
        // activando la seleccionada
        Seccion[SaveBird.SeccNum].SetActive (true);
		Secc [SaveBird.SeccNum].transform.localScale = new Vector2 (1f, 1f);
		Secc [SaveBird.SeccNum].GetComponent<CircleCollider2D> ().enabled = false;
	}
}
