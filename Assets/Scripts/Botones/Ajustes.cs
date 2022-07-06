using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ajustes : MonoBehaviour {

	public GameObject Opciones;
	public GameObject Opciones1;
	public GameObject juego;
	public static bool segur;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Volver.ejecu) {
			StartCoroutine (termi ());
			Volver.ejecu = false;
		}
	}
	void OnMouseDown(){
		if (!segur) {
			transform.localScale = new Vector3 (0.8f, 0.8f, 1f);
			StartCoroutine (opc ());
		}
	}
	IEnumerator opc(){
		yield return new WaitForSeconds (0.10f);
		transform.localScale = new Vector3 (1f, 1f, 1f);
		yield return new WaitForSeconds (0.10f);
		Opciones.SetActive (true);
		yield return new WaitForSeconds (0.99f);
		//Opciones.GetComponent<Animator> ().enabled = false;
		juego.SetActive (false);
		segur = true;
	}
	IEnumerator termi(){
		yield return new WaitForSeconds (0.97f);
		Opciones1.SetActive (false);
		segur = false;
	}
}
