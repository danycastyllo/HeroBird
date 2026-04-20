using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiaNivel : MonoBehaviour {

    // Use this for initialization
    private int[] NivSuperados;
    public GameController niveli;
    public TextMesh NiveNum;
    public int nivelNum = 0;

    public GameObject endgame;
    public GameObject finbird;

    public GameObject[] Niveles;
    private int Nivel = 0;
	private int nivelRand;
    public int[] nivPasados;
    private int i;
    //public float ts = 5f;

    void Start () {
        //nivelRand = Random.Range(0, 10);
        //Nivel = nivelRand;
        InvokeRepeating("ComprobarNivel", 0f, 22f);
    }
	
	// Update is called once per frame
	void Update () {
        if (nivelNum > 10)
        {
            finbird.transform.Translate(1f * 2f * Time.deltaTime, 0, 0);
        }

        }

    void ComprobarNivel()
    {
        //Time.timeScale = ts;
        nivelRand = Random.Range(0, 10);
        niveli.firstLevel.SetActive(false);

        if (nivelNum < 10)
        {
            for (i = 0; i < 10; i++)
            {
                if (nivPasados[i] == nivelRand)
                {
                    nivelRand = Random.Range(0, 10);
                    i = -1;
                }
            }
        }
            i = 0;
        nivPasados[nivelNum] = nivelRand;
            nivelNum++;

        Nivel = nivelRand;
        NiveNum.text = "" + nivelNum;
        niveli.firstLevel = Niveles[Nivel];
        niveli.firstLevel.SetActive(true);
        if(nivelNum > 10)
        {
            NiveNum.gameObject.SetActive(false);
            niveli.firstLevel.SetActive(false);
            StartCoroutine(deletesorpresa());
        }
    }
    IEnumerator deletesorpresa()
    {
        yield return new WaitForSeconds(8f);
        nivelNum = 9;
        finbird.SetActive(false);
        endgame.SetActive(true);
        yield return new WaitForSeconds(1.19f);
        SceneManager.LoadScene("Final");
    }
}
