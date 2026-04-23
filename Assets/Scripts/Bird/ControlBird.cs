using UnityEngine;

public class ControlBird : MonoBehaviour
{
    // ── Estado ───────────────────────────────────────────────────────────────
    public static bool isDead    = false;
    public bool canFlap   = false;

    // ── Configuración ─────────────────────────────────────────────────────────
    public float maxHeight;
    public float flapVelocity;
    public float relativeVelocityX;
    public bool move;

    // ── Referencias ───────────────────────────────────────────────────────────
    public Animator birdAnimator;
    public ParticleSystem auraParticles;

    // ── Privadas ──────────────────────────────────────────────────────────────
    Rigidbody2D rb2d;
    Collider2D col2d;
    AudioManager audioManager;
    bool isFlapping;
    float angle;

    // ─────────────────────────────────────────────────────────────────────────
    void Awake()
    {
        rb2d          = GetComponent<Rigidbody2D>();
        col2d         = GetComponent<Collider2D>();
        audioManager  = FindAnyObjectByType<AudioManager>();
    }

    void Update()
    {
        if (move)
            transform.Translate(-1 * 2f * Time.deltaTime, 0, 0);

        if (GameController.isBirdAnimating)
            birdAnimator.SetBool("Volar", isFlapping);

        birdAnimator.SetBool("Isdead", isDead);

        if (Input.GetButtonDown("Fire1") && transform.position.y < maxHeight)
        {
            if (canFlap)
            {
                audioManager.Play("Flutter");
                Flap();
            }
        }
        else
        {
            isFlapping = false;
        }

        ApplyAngle();
    }

    // ── API pública ───────────────────────────────────────────────────────────
    public bool IsDead() => isDead;

    public void Flap()
    {
        if (!move) auraParticles.Play();
        isFlapping = true;
        if (isDead) return;
        if (rb2d.bodyType == RigidbodyType2D.Kinematic) return;
        rb2d.linearVelocity = new Vector2(0f, flapVelocity);
    }

    public void SetSteerActive(bool active)
    {
        rb2d.bodyType = active ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
    }

    // ── Privadas ──────────────────────────────────────────────────────────────
    void ApplyAngle()
    {
        float targetAngle;

        if (isDead)
        {
            col2d.enabled = false;
            targetAngle   = -90f;
        }
        else
        {
            targetAngle = Mathf.Atan2(rb2d.linearVelocity.y, relativeVelocityX) * Mathf.Rad2Deg;
        }

        angle = Mathf.Lerp(angle, targetAngle, Time.deltaTime * 10f);
        transform.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    // ── Eventos ───────────────────────────────────────────────────────────────
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("puas"))
        {
            isDead   = true;
            canFlap  = false;
        }
    }

    void OnMouseDown()
    {
    }

    void OnMouseUp()
    {
    }
}