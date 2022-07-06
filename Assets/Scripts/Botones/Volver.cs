using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volver : MonoBehaviour {

	public GameObject desaparecer;
	public GameObject Opcion1;
	public GameObject juego;
	public Animator opcion;
	bool quit;
	public static bool ejecu;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		opcion.SetBool ("volver", quit);
	}
	void OnMouseDown(){
		if (Ajustes.segur) {
			transform.localScale = new Vector3 (0.8f, 0.8f, 1f);
			StartCoroutine (volv ());
		}
	}
	IEnumerator volv(){
		yield return new WaitForSeconds (0.10f);
		ejecu = true;
		transform.localScale = new Vector3 (1f, 1f, 1f);
		Opcion1.SetActive (true);
		juego.SetActive (true);
		desaparecer.SetActive (false);
	}
}
