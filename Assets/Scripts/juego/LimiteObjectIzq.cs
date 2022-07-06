using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimiteObjectIzq : MonoBehaviour {

	public float speed = 1.0f;
	public float startPosition;
	private float endPosition;

	public float minRamdon;
	public float maxRamdon;

	void Start(){
		endPosition = Random.Range (minRamdon, maxRamdon);
	}
	void Update ()
	{
		if (!ControlBird.isDead) {
			//traslacion de los objetos 
			transform.Translate (-1 * speed * Time.deltaTime, 0, 0);

			if (transform.position.x <= endPosition)
				ScrollEnd ();
		}
	}

	void ScrollEnd ()
	{
		transform.Translate(-1 * (endPosition - startPosition), 0, 0);

		SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);
	}
	IEnumerator detener(){
		yield return new WaitForSeconds (2f);
	}
}
