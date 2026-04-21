using UnityEngine;

public class LimiteObjectIzq : MonoBehaviour
{
    public float speed        = 1f;
    public float startPosition;
    public float endPosition;

    void Update()
    {
        if (!ControlBird.isDead)
        {
            transform.Translate(-speed * Time.deltaTime, 0, 0);

            if (transform.position.x <= endPosition)
                ScrollEnd();
        }
    }

    void ScrollEnd()
    {
        transform.Translate(-(endPosition - startPosition), 0, 0);
        SendMessage("OnScrollEnd", SendMessageOptions.DontRequireReceiver);
    }
}