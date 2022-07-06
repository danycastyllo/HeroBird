using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPoops : MonoBehaviour {

	public float Precio;
	public int NumPoop;

	public SavePoop guarPoop;
	public GameObject sonido;

	// Use this for initialization
	void Start () {
		if (guarPoop.Poop [NumPoop] != null) {
			if (guarPoop.Poop[NumPoop] == 1) {
				Destroy (gameObject);
			}
		}
	}

	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown(){
		if (GameController.monedas > Precio) {
			guarPoop.Poop [NumPoop] = 1;
			PlayerPrefs.SetInt ("poop" + NumPoop, guarPoop.Poop [NumPoop]);
			Destroy (gameObject);
		} else {
			print ("no Tiegnes Suficiente Dinero");
		}
	}
}
