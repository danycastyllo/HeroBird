using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {

	public GameObject pajarito;
	public Rigidbody2D pajaritoRB;
	public GameObject pre;
	public GameObject fush;
	public GameObject con;
	public GameObject[] parar;

	public bool rotalPajaro;
	public bool Movi;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (rotalPajaro) {
			pajarito.transform.Rotate (0,0,-1500f * Time.deltaTime); 
		}
	}
	void OnTriggerEnter2D(Collider2D colFruit){
		if (colFruit.gameObject.gameObject.name == "Bird") {
			Time.timeScale = 0.08f;
			parar [0].GetComponent<CircleCollider2D>().enabled = false;
			parar [1].GetComponent<CircleCollider2D>().enabled = false;
			parar [2].GetComponent<CircleCollider2D>().enabled = false;
			parar [3].GetComponent<CapsuleCollider2D>().enabled = false;
			parar [5].GetComponent<CapsuleCollider2D>().enabled = false;
			parar [2].GetComponent<SpriteRenderer>().enabled = false;
			pajarito.GetComponent<ControlBird> ().enabled = false;

			parar [4].GetComponent<CircleCollider2D>().enabled = false;
			StartCoroutine (pree ());
		}
	}
	IEnumerator pree(){
		yield return new WaitForSeconds (0.001f);
		if (Sonido.NunSoun == 1) {
			GetComponent<AudioSource> ().Play ();
		}
		Instantiate (pre, pajarito.transform.position, Quaternion.identity); 
		con.SetActive (true);
		con.transform.position = pajarito.transform.position; 
		rotalPajaro = true;
		yield return new WaitForSeconds (0.02f); 
		rotalPajaro = false;
		StartCoroutine (fushh ()); 
	}
	IEnumerator fushh(){
		yield return new WaitForSeconds (0.01f);
		Instantiate (fush, new Vector2(pajarito.transform.position.x + 1f, pajarito.transform.position.y - 0.2f), Quaternion.identity);
		yield return new WaitForSeconds (0.001f);
		Time.timeScale = 3f;
		yield return new WaitForSeconds (15f);
		Time.timeScale = 1f;
		con.SetActive (false);  
		pajarito.GetComponent<ControlBird> ().enabled = true; 
		parar [2].GetComponent<SpriteRenderer>().enabled = true; 
		yield return new WaitForSeconds (3f);
		parar [0].GetComponent<CircleCollider2D>().enabled = true; 
		parar [1].GetComponent<CircleCollider2D>().enabled = true;
		parar [2].GetComponent<CircleCollider2D>().enabled = true;
		parar [3].GetComponent<CapsuleCollider2D>().enabled = true;
		parar [4].GetComponent<CircleCollider2D>().enabled = true;
		parar [5].GetComponent<CapsuleCollider2D>().enabled = true;
	} 
}