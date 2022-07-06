using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ira : MonoBehaviour {

	public GameObject abrir;
	public GameObject cerrar;

	public bool Bajar;
	public bool devuelvis;

	public Animator subeBaja;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
	void OnMouseDown(){
		if (devuelvis) {
			Bajar = true;
			subeBaja.SetBool ("bajar", Bajar);
		}
		transform.localScale = new Vector3 (0.8f, 0.8f, 1f);
		abrir.SetActive (true);
		StartCoroutine (cierris ());
	}
	IEnumerator cierris(){
		yield return new WaitForSeconds(0.10f);
		transform.localScale = new Vector3 (1f, 1f, 1f);
		yield return new WaitForSeconds(0.30f);
		cerrar.SetActive(false);
	}
}
