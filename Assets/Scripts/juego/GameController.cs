using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // ── Estado ──────────────────────────────────────────────────────────────
    enum GameState { Menu, Playing, GameOver }
    GameState currentState;

    // ── Referencias de escena ────────────────────────────────────────────────
    public ControlBird birdScript;
    public AuraLoader auraLoader;

    // Obstáculos y generadores
    public GameObject firstLevel;
    public GameObject flappyPipe;
    public GameObject pipeGenerator;
    public GameObject rock;

    // Límites del mundo
    public GameObject floor;
    public GameObject mountains;

    // UI de gameplay
    public GameObject menuObject;
    public GameObject titleObject;
    public GameObject dataObject;
    public GameObject valuesObject;
    public GameObject levelNumberObject;

    // Pantallas de Game Over
    public GameObject[] gameOverPanels;

    // ── UI Textos ────────────────────────────────────────────────────────────
    public TextMesh textScore;
    public TextMesh textRunCoins;
    public TextMesh textTotalCoins;
    public TextMesh textRecord;
    public TextMesh textGameOverScore;
    public TextMesh textGameOverCoins;

    // ── Estado global del juego ──────────────────────────────────────────────
    public static bool isBirdAnimating = false;
    public static float currentScore   = 0;
    public static float record         = 0;
    public static int totalCoins       = 0;
    public static int runCoins         = 0;

    // ── Privadas ─────────────────────────────────────────────────────────────
    AudioManager audioManager;
    float randomSpawnDelay;

    // ────────────────────────────────────────────────────────────────────────
    void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Start()
    {
        randomSpawnDelay     = Random.Range(30f, 50f);
        totalCoins           = PlayerPrefs.GetInt(GameKeys.PlayerCoins);
        record               = PlayerPrefs.GetFloat(GameKeys.Record);
        AudioListener.volume = PlayerPrefs.GetInt(GameKeys.Sound);
        EnterMenuState();
    }

    void FixedUpdate()
    {
        textScore.text         = "" + currentScore;
        textRunCoins.text      = "" + runCoins;
        textRecord.text        = "" + record;
        textTotalCoins.text    = "" + totalCoins;
        textGameOverScore.text = "" + currentScore;
        textGameOverCoins.text = "" + runCoins;
    }

    void LateUpdate()
    {
        switch (currentState)
        {
            case GameState.Menu:
                if (Presionar.hacer == "jugar") EnterPlayState();
                break;
            case GameState.Playing:
                if (birdScript.IsDead()) EnterGameOverState();
                break;
            case GameState.GameOver:
                if (Presionar.hacer == "restart") ReloadScene();
                break;
        }
    }

    // ── Estados ───────────────────────────────────────────────────────────────
    void EnterMenuState()
    {
        runCoins     = 0;
        currentState = GameState.Menu;

        birdScript.SetSteerActive(false);
        firstLevel.SetActive(false);
        valuesObject.SetActive(false);
        pipeGenerator.SetActive(false);
        floor.GetComponent<LimiteObjectIzq>().enabled     = false;
        mountains.GetComponent<LimiteObjectIzq>().enabled = false;
        rock.GetComponent<LimiteObjectIzq>().enabled      = false;
    }

    void EnterPlayState()
    {
        birdScript.move = true;
        isBirdAnimating = true;
        currentState    = GameState.Playing;

        birdScript.SetSteerActive(true);
        StartCoroutine(WaitAndStartSpawning());

        if (ComienzoTubos.OnPerilla)
            pipeGenerator.SetActive(true);
        else
        {
            valuesObject.SetActive(true);
            levelNumberObject.SetActive(true);
        }

        floor.GetComponent<LimiteObjectIzq>().enabled      = true;
        mountains.GetComponent<LimiteObjectIzq>().enabled  = true;
        rock.GetComponent<LimiteObjectIzq>().enabled       = true;
        menuObject.GetComponent<LimiteObjectIzq>().enabled = true;
        titleObject.SetActive(false);
        dataObject.GetComponent<LimiteObjArriba>().enabled = true;
        flappyPipe.GetComponent<LimiteObjArriba>().enabled = true;

        audioManager.Stop("OpeningScene");
        audioManager.Play("BackgroundMusic");
        birdScript.Flap();
    }

    void EnterGameOverState()
    {
        audioManager.Stop("BackgroundMusic");
        audioManager.Play("Cuack");

        isBirdAnimating = false;
        currentState    = GameState.GameOver;

        CancelInvoke("metr");
        StartCoroutine(ShowGameOverPanels());

        totalCoins += runCoins;
        PlayerPrefs.SetInt(GameKeys.PlayerCoins, totalCoins);

        if (currentScore > record)
        {
            record = currentScore;
            PlayerPrefs.SetFloat(GameKeys.Record, record);
        }
    }

    void ReloadScene()
    {
        ControlBird.isDead      = false;
        ComienzoTubos.OnPerilla = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // ── Coroutines ────────────────────────────────────────────────────────────

    // Espera 3 segundos antes de activar el spawn de obstáculos
    IEnumerator WaitAndStartSpawning()
    {
        yield return new WaitForSeconds(3f);
        Destroy(rock);
        Destroy(flappyPipe);

        if (!ComienzoTubos.OnPerilla)
            firstLevel.SetActive(true);

        menuObject.SetActive(false);
        dataObject.SetActive(false);
        birdScript.move = false;

        yield return new WaitForSeconds(20f);
    }

    // Muestra los paneles de Game Over con delay entre ellos
    IEnumerator ShowGameOverPanels()
    {
        yield return new WaitForSeconds(1f);
        gameOverPanels[0].SetActive(true);
        yield return new WaitForSeconds(2.1f);
        gameOverPanels[1].GetComponent<Animator>().enabled = false;
    }
}