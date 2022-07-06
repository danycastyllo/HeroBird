using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SigienteFilaPoop : MonoBehaviour {

	public int Nump;

	public GameObject[] Seccp;
	public GameObject[] Seccionp;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown(){
		SaveBird.SeccNum = Nump;
        // desactivando todas las filas
        Seccp[0].transform.localScale = new Vector2(0.6f, 0.6f);
        Seccp[1].transform.localScale = new Vector2(0.6f, 0.6f);
        Seccp[2].transform.localScale = new Vector2(0.6f, 0.6f);
        Seccp[0].GetComponent<CircleCollider2D>().enabled = true;
        Seccp[1].GetComponent<CircleCollider2D>().enabled = true;
        Seccp[2].GetComponent<CircleCollider2D>().enabled = true;
        Seccionp[0].SetActive(false);
        Seccionp[1].SetActive(false);
        Seccionp[2].SetActive(false);
        // activando la seleccionada
        Seccionp[SaveBird.SeccNum].SetActive (true);
		Seccp [SaveBird.SeccNum].transform.localScale = new Vector2 (1f, 1f);
		Seccp [SaveBird.SeccNum].GetComponent<CircleCollider2D> ().enabled = false;
	}
}
