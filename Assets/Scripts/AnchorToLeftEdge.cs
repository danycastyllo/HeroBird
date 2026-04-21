using UnityEngine;

public class AnchorToLeftEdge : MonoBehaviour
{
    public float offsetX = 0.5f;

    void Awake()
    {
        float leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        transform.position = new Vector3(leftEdge + offsetX, transform.position.y, transform.position.z);
    }
}