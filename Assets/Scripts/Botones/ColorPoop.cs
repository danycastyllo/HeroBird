using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPoop : MonoBehaviour {

	public SaveBird sSavebird;
	public static Color coloPoop;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer> ().color = coloPoop;
	}
}
