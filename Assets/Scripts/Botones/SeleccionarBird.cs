using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionarBird : MonoBehaviour {

	public SaveBird savebird;

	public Sprite selecionado;
	public Sprite noSelecionado;
	public Sprite sprite1;
	public Sprite sprite2;
	public GameObject obj1;
	public GameObject obj2;

	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown(){
		GetComponent<SpriteRenderer> ().sprite = selecionado;
		savebird.lolo.sprite = noSelecionado;
		savebird.lolo = GetComponent<SpriteRenderer> ();
		obj1.GetComponent<SpriteRenderer> ().sprite = sprite1;
		obj2.GetComponent<SpriteRenderer> ().sprite = sprite2;
	}
}
