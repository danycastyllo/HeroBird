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
	public GameObject[] Tramp;
    public GameObject[] Gameover;
	public static bool animBird = false;
	public AuraLoader auraLoader;

	float sali;
	public static float Score = 0;
	public static float record = 0; 
	public static int playerCoins = 0; // monedas totales
	public static int runCoins = 0; // monedas en la partida 

	public TextMesh Monedas;
	public TextMesh score;
	public TextMesh DatosMonedas;
	public TextMesh DatosRecord;
	public TextMesh MonedasTienda;
	public TextMesh marcadorOver;
	public TextMesh monedasOver;

	public SpriteRenderer SelectedBird; //the current skin of the selected bird
	public SpriteRenderer SelectedPoop; //the current skin of the selected poop
	public static int SeccNum;



	// Use this for initialization
	void Start () {
        Menu ();
		sali = Random.Range (30f, 50f);
		playerCoins = PlayerPrefs.GetInt ("PlayerCoins");
		record = PlayerPrefs.GetFloat ("record");
		AudioListener.volume = PlayerPrefs.GetInt ("Sound"); // devuelve el playerprefs del sonido para determinar si esta mute
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		score.text = "" + Score;
		Monedas.text = "" + runCoins;
		DatosRecord.text = "" + record;
		DatosMonedas.text = "" + playerCoins;
		marcadorOver.text = "" + Score;
		monedasOver.text = "" + runCoins;

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
		runCoins = 0;
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
		birdScript.move = true;
		animBird = true;
		state = State.Play;
		birdScript.SetSteerActive(true);
		StartCoroutine (prime ());

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
		FindAnyObjectByType<AudioManager>().Stop("OpeningScene");
		FindAnyObjectByType<AudioManager>().Play("BackgroundMusic");
        Tramp[3].GetComponent<LimiteObjArriba>().enabled = true;
        //alert.enabled = true;

        
        birdScript.Flap();
	}

	void GameOver ()
	{
		FindAnyObjectByType<AudioManager>().Stop("BackgroundMusic");
		FindAnyObjectByType<AudioManager>().Play("Cuack"); // death sound
		animBird = false;
		StartCoroutine (restartt ());
		CancelInvoke ("metr");
		state = State.GameOver;
		playerCoins += runCoins; // suma las monedas ganadas en la partida al total de monedas obtenidas
		PlayerPrefs.SetInt ("PlayerCoins", playerCoins); // guarda el acumulado de monedas (en playerprefs)

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
