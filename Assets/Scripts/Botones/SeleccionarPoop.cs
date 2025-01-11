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
	private GameController gameController;

	// Use this for initialization
	void Start () {
		//We get the component where the current skin variable of the selected poop is located
		gameController = GameObject.Find("GameControl").GetComponent<GameController>();
	}

	// Update is called once per frame
	void Update () {

	}
	void OnMouseDown(){
		GetComponent<SpriteRenderer> ().sprite = selecionado;
		gameController.SelectedPoop.sprite = NoSelecionado;
		gameController.SelectedPoop = GetComponent<SpriteRenderer> ();
		obj1.GetComponent<SpriteRenderer> ().sprite = spritePoop;
		obj2.GetComponent<SpriteRenderer> ().sprite = spritePoop;
		obj3.GetComponent<SpriteRenderer> ().sprite = spritePoop;
	}
}
