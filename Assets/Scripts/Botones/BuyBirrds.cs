using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyBirrds : MonoBehaviour {

	public float Precio;
	public int NumBird;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt ("bird" + NumBird, 0) == 1) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown(){
		if (GameController.monedas >= Precio) {
			PlayerPrefs.SetInt ("bird" + NumBird, 1);
			GameController.monedas -= Precio;
			Destroy (gameObject);
			PlayerPrefs.SetFloat ("monedas", GameController.monedas);
        } else {
			print ("No Tienes Suficiente Monedas");
		}
	}
}
