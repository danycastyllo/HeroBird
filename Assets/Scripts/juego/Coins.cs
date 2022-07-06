using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour {

	public bool MonedaC;
	private Animator coin;
	// Use this for initialization
	void Start () {
		Sonido.NunSoun = PlayerPrefs.GetInt ("Son");
		coin = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (MonedaC) {
			coin.SetBool ("Cojida", MonedaC);
			StartCoroutine(coinsEnun());
		}
	}
	void OnTriggerEnter2D(Collider2D ColM){
		if (ColM.gameObject.name == "Bird") {
			if (Sonido.NunSoun == 1) {
				GetComponent<AudioSource> ().Play ();
			}
			MonedaC = true;
			GameController.monedas += 1f;
			GameController.monedasDePartida += 1f;
			PlayerPrefs.SetFloat ("monedas", GameController.monedas);
		}
	}
	IEnumerator coinsEnun(){
		yield return new WaitForSeconds (0.30f);
		Destroy (gameObject);
	}
}

