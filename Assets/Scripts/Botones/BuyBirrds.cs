using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyBirrds : MonoBehaviour {

	public float Precio;
	public int NumBird;
	public GameObject sonido;

	public SaveBird guarBird;

	// Use this for initialization
	void Start () {
		if (guarBird.Bird [NumBird] != null) {
			if (guarBird.Bird [NumBird] == 1) {
				Destroy (gameObject);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown(){
		if (GameController.monedas > Precio) {
			guarBird.Bird [NumBird] = 1;
			PlayerPrefs.SetInt ("bird" + NumBird, guarBird.Bird [NumBird]);
			GameController.monedas -= Precio;
			Destroy (gameObject);
			PlayerPrefs.SetFloat ("monedas", GameController.monedas);
        } else {
			print ("no Tiegnes Suficiente Dinero");
		}
	}
}
