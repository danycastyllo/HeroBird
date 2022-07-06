using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdPoop : MonoBehaviour {

	public GameObject ventana1;
	public GameObject poops;
	public GameObject birds;
    public GameObject gamecon;
    public Sprite claro;
	public Sprite oscuro;
	public string QueEs;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
    }
    void FixedUpdate()
    {
    }
    void OnMouseDown(){
		GetComponent<SpriteRenderer> ().sprite = claro;
		transform.localPosition = new Vector3(transform.position.x, transform.position.y, -2f);
		ventana1.GetComponent<SpriteRenderer> ().sprite = oscuro;
		ventana1.transform.localPosition = new Vector3(ventana1.transform.position.x, ventana1.transform.position.y, -1f);
		if (QueEs == "birds") {
			birds.SetActive (true);
			poops.SetActive (false);
		} else if(QueEs == "poops") {
			poops.SetActive (true);
			birds.SetActive (false);
		}
        SaveBird.SeccNum = 0;
        SavePoop.SeccPoop = 0;
	}
}
