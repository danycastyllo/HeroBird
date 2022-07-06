using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeleccionarPoop : MonoBehaviour {

	public Sprite selecionado;
	public Sprite NoSelecionado;
	public Sprite spritePoop;
	public GameObject obj1;
	public GameObject obj2;
	public GameObject obj3;
	public SavePoop savepoop;

	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll ();
	}

	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown(){
		GetComponent<SpriteRenderer> ().sprite = selecionado;
		savepoop.lolu.sprite = NoSelecionado;
		savepoop.lolu = GetComponent<SpriteRenderer> ();
		obj1.GetComponent<SpriteRenderer> ().sprite = spritePoop;
		obj2.GetComponent<SpriteRenderer> ().sprite = spritePoop;
		obj3.GetComponent<SpriteRenderer> ().sprite = spritePoop;
	}
}
