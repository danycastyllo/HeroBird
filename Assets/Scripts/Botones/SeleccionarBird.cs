using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionarBird : MonoBehaviour {

	public Sprite selecionado;
	public Sprite noSelecionado;
	public Sprite sprite1;
	public Sprite sprite2;
	public GameObject obj1;
	public GameObject obj2;
	private GameController gameController;

	// Use this for initialization
	void Start () {
		//We get the component where the current skin variable of the selected bird is located
		gameController = GameObject.Find("GameControl").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown(){
		GetComponent<SpriteRenderer> ().sprite = selecionado;
		gameController.SelectedBird.sprite = noSelecionado;
		gameController.SelectedBird = GetComponent<SpriteRenderer> ();
		obj1.GetComponent<SpriteRenderer> ().sprite = sprite1;
		obj2.GetComponent<SpriteRenderer> ().sprite = sprite2;
	}
}
