using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour {

	public bool MonedaC;
	private Animator coin;
	public AudioClip coinSound;
	// Use this for initialization
	void Start () {
		coin = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	private void CollectCoin()
    {
	    
		AudioPool.Instance.PlaySound(coinSound); // intancia la funcion que crea el numero de objetos para los audios de las monedas
		GameController.monedasDePartida += 1f; // aumenta el contador de monedas obtenidas en la partida actual

		// activa la animacion de obtencion de la moneda y luego la destruye
		MonedaC = true;
		coin.SetBool ("Cojida", MonedaC);
		StartCoroutine(coinsEnun());
    }
	void OnTriggerEnter2D(Collider2D ColM){
		if (ColM.gameObject.name == "Bird") {
			CollectCoin();
		}
	}
	IEnumerator coinsEnun(){
		yield return new WaitForSeconds (0.40f);
		Destroy (gameObject);
	}
}

