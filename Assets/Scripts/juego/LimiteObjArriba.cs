using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimiteObjArriba : MonoBehaviour {

	public float speed = 1.0f;

	void Start(){
	}
	void Update ()
	{
		if (!ControlBird.isDead) {
			//traslacion de los objetos 
			transform.Translate (0, 1 * speed * Time.deltaTime, 0);
		}
	}
}
