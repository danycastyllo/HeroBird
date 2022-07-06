using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class subebaja : MonoBehaviour {

	bool sube = true;
	public float peed;
	// Use this for initialization
	void Start () {
		StartCoroutine (suja ());
	}
	
	// Update is called once per frame
	void Update () {
		if (sube == false) {
			transform.Translate (0, -1 * peed * Time.deltaTime, 0);
			print ("baja");

		} else {
			transform.Translate (0, 1 * peed * Time.deltaTime, 0);
			print ("sube");
		}
	}
	IEnumerator suja(){
		yield return new WaitForSeconds (1f);
		if (sube == true) {
			sube = false;
		} else {
			sube = true;
		}
		StartCoroutine (suja ());
	}
	//IEnumerator subee(){
	//	yield return new WaitForSeconds (1f);

	//}
}
