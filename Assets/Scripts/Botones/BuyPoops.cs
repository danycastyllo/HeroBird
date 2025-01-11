using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPoops : MonoBehaviour {

	public float Precio;
	public int NumPoop;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("poop" + NumPoop, 0) == 1) {
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown(){
		if (GameController.monedas > Precio) {
			PlayerPrefs.SetInt ("poop" + NumPoop, 1);
			Destroy (gameObject);
		} else {
			print ("no Tienes Suficiente Monedas");
		}
	}
}
