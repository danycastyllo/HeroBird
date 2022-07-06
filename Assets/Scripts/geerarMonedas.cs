using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class geerarMonedas : MonoBehaviour {

	public GameObject[] coines;
	public float tiempoMin = 15f;
	public float tiempoMax = 20f;
	// Use this for initialization
	void Start () {
		GeneCoins ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void GeneCoins(){
		Instantiate (coines [Random.Range (0, coines.Length)], transform.position, Quaternion.identity);
		Invoke ("GeneCoins", Random.Range (tiempoMin, tiempoMax));
	}
}
