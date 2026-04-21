using UnityEngine;

public class MoverIzq : MonoBehaviour
{
    public float speed;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bird")
        {
            audioSource.Play();
            GameController.currentScore++;
        }
    }
}