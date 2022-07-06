using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class loading : MonoBehaviour
{
    public Transform LoadingBar;
    public Transform TextIndicator;
    [SerializeField] private float CantidadActual;
    [SerializeField] private float VeloBarra;

    void Start()
    {
        
    }
    

    void Update()
    {
        if(CantidadActual < 100)
        {
            CantidadActual += VeloBarra * Time.deltaTime;
            TextIndicator.GetComponent<UnityEngine.UI.Text>().text = ((int)CantidadActual).ToString() + "%";
        }
        else
        {
            TextIndicator.GetComponent<UnityEngine.UI.Text>().text = "listo";
            SceneManager.LoadScene(1);
        }
        LoadingBar.GetComponent<UnityEngine.UI.Image>().fillAmount = CantidadActual / 120 + 0.08f;
    }
}
