using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alerta : MonoBehaviour {

	public GameObject pajaro;
	public GameObject halcon;

	public bool ayu;
	public bool halconAttack;
	Animator anin;
	Animator halconAnim;

	float alea;
	// Use this for initialization
	void Start () {
		alea = Random.Range (25f, 30f);
		halconAnim = halcon.GetComponent<Animator> ();
		anin = GetComponent<Animator> ();
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		InvokeRepeating ("generar", alea, alea);
	}
	
	// Update is called once per frame
	void Update () {
		anin.SetBool ("Ayu", ayu);

		gameObject.transform.position = new Vector3(transform.position.x, pajaro.transform.position.y, -5f);

		if (halcon.transform.position.x < -30f) {
			print ("hol");
			halcon.transform.position = new Vector2(18f, transform.position.y);
			halcon.SetActive (false);
		}
	}

	IEnumerator hal(){
		yield return new WaitForSeconds (5f);
		halcon.SetActive (true);
		halcon.transform.position = new Vector2(halcon.transform.position.x, transform.position.y);
		gameObject.GetComponent<SpriteRenderer> ().enabled = false;
		ayu = false;
		StartCoroutine (aniHalcon ());
	}
	IEnumerator aniHalcon(){
		yield return new WaitForSeconds (0.40f);
		halconAttack = true;
		halconAnim.SetBool ("attack", halconAttack);
	}
	void generar(){
		gameObject.GetComponent<SpriteRenderer> ().enabled = true;
		if (Sonido.NunSoun == 1) {
			GetComponent<AudioSource> ().Play ();
		}
		ayu = true;
		halconAttack = false;
		StartCoroutine (hal ());
	}
}
