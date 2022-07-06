using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruir : MonoBehaviour {

	public float TiemDes;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, TiemDes);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
