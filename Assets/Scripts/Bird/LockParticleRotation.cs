using UnityEngine;

public class LockParticleRotation : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.identity; // Bloquea la rotación
    }
}
