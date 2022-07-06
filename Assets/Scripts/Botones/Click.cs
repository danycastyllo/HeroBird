using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour {

	public static bool clic;
	public static bool buy;
	public AudioClip buyy;
	public AudioClip selec;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (buy & clic) {
			if (Sonido.NunSoun == 1) {
				GetComponent<AudioSource> ().clip = buyy;
				GetComponent<AudioSource> ().Play ();
			}
			
		} else if (buy = false & clic) {
			if (Sonido.NunSoun == 1) {
				GetComponent<AudioSource> ().clip = selec;
				GetComponent<AudioSource> ().Play ();
			}
		}
	}
}
