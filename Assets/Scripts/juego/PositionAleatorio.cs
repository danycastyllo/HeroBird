using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAleatorio : MonoBehaviour {

	public float minHeight;
	public float maxHeight;
	public GameObject pivot;

	void Start () 
	{
		// 시작할 때 틈새의 높이를 변경
		ChangeHeight();
	}

	void ChangeHeight ()
	{ 
		// 임의의 높이를 생성하고 설정
		float height = Random.Range(minHeight, maxHeight);
		pivot.transform.localPosition = new Vector3(gameObject.transform.position.x, height, 0);
	}

	// ScrollObject 스크립트로부터의 메시지를 받아 높이를 변경
	void OnScrollEnd ()
	{
		ChangeHeight();
	}
}
