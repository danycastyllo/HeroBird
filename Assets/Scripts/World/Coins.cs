using System.Collections;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public AudioClip coinSound;

    Animator coinAnimator;

    void Start()
    {
        coinAnimator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bird")
            CollectCoin();
    }

    void CollectCoin()
    {
        AudioPool.Instance.PlaySound(coinSound);
        GameController.runCoins++;
        coinAnimator.SetBool("Cojida", true);
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(0.40f);
        Destroy(gameObject);
    }
}