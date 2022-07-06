using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotar : MonoBehaviour {
	
	public float Speed; 

	void Update () 
	{
		transform.Rotate (0,0,1* Speed * Time.deltaTime); 
	}
}