using UnityEngine;

public class Rotar : MonoBehaviour
{
    public float speed;

    void Update()
    {
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}