using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiaNivel : MonoBehaviour
{
    // ── Referencias ───────────────────────────────────────────────────────────
    public GameController gameController;
    public TextMesh levelNumberText;
    public GameObject[] levels;
    public GameObject endGameScreen;
    public GameObject finalBird;

    // ── Estado ────────────────────────────────────────────────────────────────
    public int currentLevelIndex = 0;
    public int[] completedLevels;

    // ── Privadas ──────────────────────────────────────────────────────────────
    int randomLevel;
    const int maxLevels     = 10;
    const float levelDelay  = 22f;

    // ─────────────────────────────────────────────────────────────────────────
    void Start()
    {
        InvokeRepeating(nameof(CheckAndLoadNextLevel), 0f, levelDelay);
    }

    void Update()
    {
        if (currentLevelIndex > maxLevels)
            finalBird.transform.Translate(2f * Time.deltaTime, 0, 0);
    }

    // ── Lógica de niveles ─────────────────────────────────────────────────────

    // Selecciona un nivel aleatorio que no haya sido jugado recientemente
    void CheckAndLoadNextLevel()
    {
        randomLevel = Random.Range(0, maxLevels);
        gameController.firstLevel.SetActive(false);

        if (currentLevelIndex < maxLevels)
        {
            for (int i = 0; i < maxLevels; i++)
            {
                if (completedLevels[i] == randomLevel)
                {
                    randomLevel = Random.Range(0, maxLevels);
                    i = -1;
                }
            }
        }

        completedLevels[currentLevelIndex] = randomLevel;
        currentLevelIndex++;

        levelNumberText.text          = "" + currentLevelIndex;
        gameController.firstLevel     = levels[randomLevel];
        gameController.firstLevel.SetActive(true);

        if (currentLevelIndex > maxLevels)
        {
            levelNumberText.gameObject.SetActive(false);
            gameController.firstLevel.SetActive(false);
            StartCoroutine(ShowEndSequence());
        }
    }

    // Secuencia final — muestra pantalla de fin y carga escena final
    IEnumerator ShowEndSequence()
    {
        yield return new WaitForSeconds(8f);
        currentLevelIndex = 9;
        finalBird.SetActive(false);
        endGameScreen.SetActive(true);
        yield return new WaitForSeconds(1.19f);
        SceneManager.LoadScene("Final");
    }
}