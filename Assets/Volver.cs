using System.Collections;
using UnityEngine;

public class Volver : MonoBehaviour
{
    public GameObject settingsPanel;
    public GameObject gamePanel;
    public Animator settingsAnimator;
    public static bool ejecu;

    void OnMouseDown()
    {
        if (Ajustes.isOpen)
        {
            transform.localScale = new Vector3(0.8f, 0.8f, 1f);
            StartCoroutine(CloseSettings());
        }
    }

    IEnumerator CloseSettings()
    {
        yield return new WaitForSeconds(0.10f);
        ejecu = true;
        settingsAnimator.SetBool("volver", ejecu);
        transform.localScale = Vector3.one;
        gamePanel.SetActive(true);
        yield return new WaitForSeconds(1f);
        settingsAnimator.Rebind();
        settingsPanel.SetActive(false);
    }
}