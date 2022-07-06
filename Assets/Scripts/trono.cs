using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trono : MonoBehaviour {

	public LimiteObjectIzq deten;
	public GameObject bid;
	public bool pase = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetButtonDown ("Fire1") && deten.enabled == false && ControlBird.Aleteo == true){
			deten.enabled = true;
		}
		
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name == "Bird") {


			deten.enabled = false;
			// lo que pasa es q se llama muchas veces este caso y se buguea hay q buscar la forma de que esto solo ocurra una vez
			if (pase == true) {
				bid.transform.position = new Vector2 (gameObject.transform.position.x, bid.transform.position.y);
				pase = false;
				StartCoroutine (pas ()); 
			}
		}
	}
	IEnumerator pas(){
		yield return new WaitForSeconds (1f);
		pase = true;
	}
}
