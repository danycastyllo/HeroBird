using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class quitarSound : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Sonido.NunSoun = PlayerPrefs.GetInt ("Son");
		if (Sonido.NunSoun != 1) {
			GetComponent<AudioSource> ().Stop (); 
		} else {
			GetComponent<AudioSource> ().Play ();
		}
	}
	
	// Update is called once per frame
	void Update () { 
	
	}
}
