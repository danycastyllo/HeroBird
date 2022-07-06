using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sonido : MonoBehaviour {

	public GameObject On;
	public GameObject Off;

	public static bool soun = true;
	public static int NunSoun = 1;

	public GameObject[] apagar;

	// Use this for initialization
	void Start () {
		NunSoun = PlayerPrefs.GetInt ("Son");
		if (NunSoun == 1) {
			soun = true;
			Off.SetActive (false);
			On.SetActive (true);
			//apagar [0].GetComponent<AudioSource> ().Play ();
		} else {
			soun = false;
			Off.SetActive (true);
			On.SetActive (false);
            apagar[0].GetComponent<AudioSource>().Stop();
            apagar[1].GetComponent<AudioSource>().Stop();
            apagar[2].GetComponent<AudioSource>().Stop();
            apagar[3].GetComponent<AudioSource>().Stop();
            apagar[4].GetComponent<AudioSource>().Stop();
            apagar[5].GetComponent<AudioSource>().Stop();
        }
	}

	// Update is called once per frame
	void Update () {
	}
	void OnMouseDown(){
		this.gameObject.transform.localScale = new Vector3 (1.2f, 1.2f, 1f);
		if (!soun) {
			Off.SetActive (false);
			On.SetActive (true);
			soun = true;
			NunSoun = 1;
			PlayerPrefs.SetInt ("Son", NunSoun);
            //apagar [0].GetComponent<AudioSource> ().Play (); 
            apagar[2].GetComponent<AudioSource>().Play();
        }
        else{
			Off.SetActive (true);
			On.SetActive (false);
			soun = false;
			NunSoun = 2;
			PlayerPrefs.SetInt ("Son", NunSoun);
			apagar[0].GetComponent<AudioSource>().Stop(); 
			apagar[1].GetComponent<AudioSource>().Stop();
            apagar[2].GetComponent<AudioSource>().Stop();
            apagar[3].GetComponent<AudioSource>().Stop();
            apagar[4].GetComponent<AudioSource>().Stop();
            apagar[5].GetComponent<AudioSource>().Stop(); 
        }
	}
	void OnMouseUp(){

		this.gameObject.transform.localScale = new Vector3 (1f, 1f, 1f);
	}
}
