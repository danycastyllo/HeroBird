using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//                                                                                tipo de juego completar palabras letra por letra en 3 caminos y sumas y restas
public class GameController : MonoBehaviour {

	enum State
	{
		Menu,
		Play,
		GameOver
	} 

	State state;

	public ControlBird birdScript;
	public Alerta alert;
	public GameObject[] Tramp;
    public GameObject[] Gameover;
	public static bool animBird = false;

	float sali;
	public static float Score = 0;
	float Por = 1;
	public static float record = 0; 
	public static float monedas = 0;
	public static float monedasDePartida = 0;

	public TextMesh Monedas;
	public TextMesh score;
	public TextMesh DatosMonedas;
	public TextMesh DatosRecord;
	public TextMesh MonedasTienda;
	public TextMesh marcadorOver;
	public TextMesh monedasOver;

	// Use this for initialization
	void Start () {
        Menu ();
		sali = Random.Range (30f, 50f);
		monedas = PlayerPrefs.GetFloat ("monedas");
		record = PlayerPrefs.GetFloat ("record");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		score.text = "" + Score;
		Monedas.text = "" + monedasDePartida;
		DatosRecord.text = "" + record;
		DatosMonedas.text = "" + monedas;
		MonedasTienda.text = "" + monedas;
		marcadorOver.text = "" + Score;
		monedasOver.text = "" + monedasDePartida;

	}
	void LateUpdate ()
	{
		switch (state)
		{
		case State.Menu: 
			if (Presionar.hacer == "jugar") GameStart();
			break;
		case State.Play:
			if (birdScript.IsDead()) GameOver();
			break;
		case State.GameOver:
			if (Presionar.hacer == "restart") Reload();
			break;
		}
	}
	void Menu ()	
	{
		monedasDePartida = 0;
		state = State.Menu;

		// 각 오브젝트를 무효 상태로 한다
		birdScript.SetSteerActive(false);

		Tramp[0].SetActive(false);
		//Tramp[1].SetActive(false);
		//Tramp[2].SetActive(false);
		//Tramp[3].SetActive(false);
		Tramp[7].SetActive(false);

		Tramp[8].SetActive(false);
		Tramp [4].GetComponent<LimiteObjectIzq> ().enabled = false;
		Tramp [5].GetComponent<LimiteObjectIzq> ().enabled = false;
		Tramp [6].GetComponent<LimiteObjectIzq> ().enabled = false;

		//alert.enabled = false;
	}

	void GameStart ()
	{
		animBird = true;
		state = State.Play;
		birdScript.SetSteerActive(true);
		StartCoroutine (prime ());
		birdScript.move = true;
        if(ComienzoTubos.OnPerilla == true) {
            Tramp[8].SetActive(true);
        }
        else
        {
            Tramp[7].SetActive(true);
            Tramp[12].SetActive(true);
        }
		Tramp [4].GetComponent<LimiteObjectIzq> ().enabled = true;
		Tramp [5].GetComponent<LimiteObjectIzq> ().enabled = true;
		Tramp [6].GetComponent<LimiteObjectIzq> ().enabled = true;
		Tramp [9].GetComponent<LimiteObjectIzq> ().enabled = true;
		Tramp [10].SetActive (false);
		Tramp [11].GetComponent<LimiteObjArriba> ().enabled = true;
        Tramp[3].GetComponent<LimiteObjArriba>().enabled = true;
        //alert.enabled = true;

        
        birdScript.Flap();
	}

	void GameOver ()
	{
		if (Sonido.NunSoun == 1) {
			GetComponent<AudioSource> ().Play ();
		}
		animBird = false;
		StartCoroutine (restartt ());
		CancelInvoke ("metr");
		state = State.GameOver;
		//Tramp[2].SetActive(false);
		//ScrollObject[] scrollObjects = GameObject.FindObjectsOfType<ScrollObject>();
		//foreach (ScrollObject so in scrollObjects) so.enabled = false;
		if (Score > record) {
			record = Score;
			PlayerPrefs.SetFloat ("record", record);
		}

	}
	void Reload ()
	{
		ControlBird.isDead = false;
            ComienzoTubos.OnPerilla = false;
		//Application.LoadLevel(Application.loadedLevel);  //5.2
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);   //5.3
	}
	IEnumerator tiem(){
		yield return new WaitForSeconds (5f);
		//Tramp[3].SetActive(true);
		yield return new WaitForSeconds (sali);
		//Tramp[1].SetActive(true);
	}
	IEnumerator prime(){
		yield return new WaitForSeconds (3f);
		Destroy (Tramp [6]);
        Destroy(Tramp[3]);
        if (ComienzoTubos.OnPerilla != true)
        {
            Tramp[0].SetActive(true);
        }
		StartCoroutine (tiem ());
		//Tramp[2].SetActive(true);
		//Tramp[8].SetActive(true);
		Tramp [9].SetActive (false);
		Tramp [11].SetActive (false);
		birdScript.move = false;
		yield return new WaitForSeconds (20f);
	}

	IEnumerator restartt(){
		yield return new WaitForSeconds (1f);
		Gameover [0].SetActive (true);
		yield return new WaitForSeconds (2.1f);
		Gameover [1].GetComponent<Animator> ().enabled = false;
	}

}
