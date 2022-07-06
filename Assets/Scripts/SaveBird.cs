using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBird : MonoBehaviour {

	//public static int selecionado = 2;
	public SpriteRenderer lolo;
	public static int SeccNum;
	public int[] Bird;
	// Use this for initialization
	void Start () {
        for (int i = 0; i <= Bird.Length -1f; i++) {
			Bird [i] = PlayerPrefs.GetInt ("bird" + i, 0);
		}
	}
	
	// Update is called once per frame
	void Update ()
    {

    }
}
