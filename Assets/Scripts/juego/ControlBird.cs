using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBird : MonoBehaviour {

	Rigidbody2D rb2d;
	public static bool isDead;
	float angle;

	public float maxHeight;
	public float flapVelocity;
	public float relativeVelocityX;
	public GameObject Pajaro;
	public Rigidbody2D Halcon;

	public bool volar;
	public static bool Jugar = false;
	public static bool Aleteo = false;
	public bool move;
	public Animator bird;
	// Use this for initialization
	void Start(){
	}
	public bool IsDead ()
	{
		return isDead;
	}
	void Awake ()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}
	void Update ()
	{
		if (move) {
			transform.Translate (-1 * 2f * Time.deltaTime, 0, 0);
		}
		// se activa la animacion de aleteo
		if (GameController.animBird == true) {
			bird.SetBool ("Volar", volar);
		}
		bird.SetBool ("Isdead", isDead);
		// se llama a el metodo flap si se preciona en la pantalla
		if (Input.GetButtonDown ("Fire1") && transform.position.y < maxHeight) {

			if (Aleteo == true) {
				if(Sonido.NunSoun == 1){
				GetComponent<AudioSource> ().Play ();
				}
			}
			Flap ();
		} else {
			volar = false;
		}
		ApplyAngle();
	}

	public void Flap ()
	{
		volar = true;
		if(isDead) return;
		// 중력을 받지 않을 때는 조작하지 않는다
		if(rb2d.isKinematic) return;

		// Velocity를 직접 바꿔 써서 위쪽 방향으로 가속
		rb2d.velocity = new Vector2(0.0f, flapVelocity);
	}

	void ApplyAngle ()
	{
		// 현재 속도, 상대 속도로부터 진행되고 있는 각도를 구한다
		float targetAngle;
		// caundo muera se rota de pico abajo
		if (isDead)
		{
			targetAngle = -90.0f;
		}
		else
		{
			targetAngle = 
				Mathf.Atan2(rb2d.velocity.y, relativeVelocityX) * Mathf.Rad2Deg;
		}

		// 회전 애니메이션을 스무딩
		angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10.0f);

		// Rotation의 반영
		Pajaro.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, angle);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "puas") {
			isDead = true;
			Aleteo = false;

			if (isDead)
				return;
		}
			
	}
	void OnMouseDown(){
		Jugar = true;
		Aleteo = true;
	}
	void OnMouseUp(){
		Jugar = false;
	}

	public void SetSteerActive (bool active)
	{
		rb2d.isKinematic = !active;
	}
}
