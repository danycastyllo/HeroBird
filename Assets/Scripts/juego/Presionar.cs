using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presionar : MonoBehaviour {

	public static string hacer = "nada";
	public string va; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnMouseDown(){
		hacer = va;
	}
	void OnMouseUp(){
		hacer = "nada";
	}
}
