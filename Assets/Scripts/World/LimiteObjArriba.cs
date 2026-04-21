using UnityEngine;

public class LimiteObjArriba : MonoBehaviour
{
    public float speed = 1f;

    void Update()
    {
        if (!ControlBird.isDead)
            transform.Translate(0, speed * Time.deltaTime, 0);
    }
}