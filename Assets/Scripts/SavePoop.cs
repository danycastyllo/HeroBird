using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoop : MonoBehaviour {

	public static int SeccPoop;
	public SpriteRenderer lolu;
	public int[] Poop;
	// Use this for initialization
	void Start () {
		for (int i = 0; i <= Poop.Length -1f; i++) {
			Poop [i] = PlayerPrefs.GetInt ("poop" + i, 0);
		}
	}

	// Update is called once per frame
	void Update () {
       // print(SeccPoop);

    }
}
