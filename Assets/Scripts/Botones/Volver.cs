using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volver : MonoBehaviour {

	public GameObject desaparecer;
	public GameObject juego;
	public Animator opcion;
	public static bool ejecu;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
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
		opcion.SetBool ("volver", ejecu);
		transform.localScale = new Vector3 (1f, 1f, 1f);
		juego.SetActive (true);
		
		yield return new WaitForSeconds (1f);
		opcion.Rebind(); // restaura el Animator a su estado inicial (restablece todos los parámetros a sus valores predeterminados)
		desaparecer.SetActive (false);

	}
}
