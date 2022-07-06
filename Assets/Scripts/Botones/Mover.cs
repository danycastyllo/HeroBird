using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

	public static float startPosition = -11f;
	public static float endPosition = -0.5f;

	public static float posic = 1f;
	public static float sspeed = 30f;


	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (0, startPosition, -5);
	}

	// Update is called once per frame
	void Update ()
	{
		if (!ControlBird.isDead) {
			//traslacion de los objetos 

			// 스크롤이 목표 지점까지 도달했는지 체크
			if (transform.position.y >= endPosition && posic > 0) { 
				transform.Translate (0, 0, 0);
			} else if (transform.position.y >= endPosition && posic < 0) { 
				transform.Translate (0, 0, 0);  
			} else {
				transform.Translate (0, posic * sspeed * Time.deltaTime, 0);
			}

		}
	}
}
