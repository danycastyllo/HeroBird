using System.Collections;
using UnityEngine;

public class ComienzoTubos : MonoBehaviour
{
    public GameObject shadow;
    public float rotationSpeed;
    public static bool OnPerilla = false;

    void Update()
    {
        if (OnPerilla)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            shadow.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    void OnMouseDown()
    {
        OnPerilla = true;
        StartCoroutine(TriggerGameStart());
    }

    IEnumerator TriggerGameStart()
    {
        yield return new WaitForSeconds(2f);
        Presionar.hacer = "jugar";
        yield return new WaitForSeconds(0.2f);
        Presionar.hacer = "nada";
    }
}