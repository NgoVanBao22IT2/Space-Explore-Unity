using UnityEngine;
using System.Collections;


public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private Material whiteMaterial;
    private FlashWhite flashWhite;
    [SerializeField] private GameObject destroyEffect;

    private int lives;
    private int maxLives;
    private int damage;
    [SerializeField] private int experienceToGive = 5; // Asteroid cho nhiều experience hơn
    private Material originalMaterial;

    void OnEnable()
    {
        lives = maxLives;
        transform.rotation = Quaternion.identity;
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        rb = GetComponent<Rigidbody2D>();
        flashWhite = GetComponent<FlashWhite>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        float pushX = Random.Range(-1f, 0);
        float pushY = Random.Range(-1f, 1f);
        rb.linearVelocity = new Vector2(pushX, pushY);
        float randomScale = Random.Range(0.6f, 1f);
        transform.localScale = new Vector2(randomScale, randomScale);

        maxLives = 5;
        lives = maxLives;
        damage = 1;

    }

    void Update()
    {
        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player)
            {
                player.TakeDamage(damage);
                // Giảm weapon level khi va chạm
                PhaserWeapon.Instance.LevelDown();
            }
        }
    }

    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = originalMaterial;
    }

    public void TakeDamage(int damage)
    {
        lives -= damage;

        if (lives > 0)
        {
            if (flashWhite != null)
                flashWhite.Flash();
        }
        else
        {
            // Tạo hiệu ứng nổ nếu có destroyEffect prefab
            if (destroyEffect != null)
            {
                GameObject effect = Instantiate(destroyEffect, transform.position, transform.rotation);
                effect.transform.localScale = transform.localScale;
                // Tự động destroy effect sau 2 giây
                Destroy(effect, 2f);
            }

            if (spriteRenderer != null && originalMaterial != null)
                spriteRenderer.material = originalMaterial;

            if (PlayerController.Instance != null)
                PlayerController.Instance.GetExperience(experienceToGive);

            Destroy(gameObject);
        }

    }
}
