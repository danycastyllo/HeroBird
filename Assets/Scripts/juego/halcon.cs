using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class halcon : MonoBehaviour {

	public Rigidbody2D pjaroo;
	bool choco;

	// Use this for initialization
	void Start () {
		Sonido.NunSoun = PlayerPrefs.GetInt ("Son");
		if (Sonido.NunSoun != 1) {
			GetComponent<AudioSource> ().Stop ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (choco) {
			pjaroo.AddForce (Vector2.left * Time.deltaTime * 20f, ForceMode2D.Impulse);
			gameObject.GetComponent<Rigidbody2D> ().isKinematic = false;
			gameObject.GetComponent<CircleCollider2D> ().isTrigger = false;
			gameObject.GetComponent<Rigidbody2D>().AddForce (Vector2.left * Time.deltaTime * 20f, ForceMode2D.Impulse);
		}
	}
	void OnTriggerEnter2D (Collider2D collh)
	{
		if (collh.gameObject.name == "Bird") {
			choco = true;
			StartCoroutine (cho ());
		}

	}
	IEnumerator cho(){
		yield return new WaitForSeconds (3f);
		choco = false;
	}
}
