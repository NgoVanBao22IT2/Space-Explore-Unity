using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Rigidbody2D rb;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;


    private Vector2 playerDirection;
    [SerializeField] private float moveSpeed;
    public float boost = 1f;
    private float boostPower = 4f;
    private bool boosting = false;

    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRegen;
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private ParticleSystem engineEffect;

    [SerializeField] private int experience;
    [SerializeField] private int currentLevel;
    [SerializeField] private int maxLevel;
    [SerializeField] private List<int> playerLevels;





    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        for (int i = playerLevels.Count; i < maxLevel; i++)
        {
            playerLevels.Add(Mathf.CeilToInt(playerLevels[playerLevels.Count - 1] * 1.1f + 15));
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        energy = maxEnergy;
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
        health = maxHealth; // Sử dụng giá trị maxHealth đã set trong Inspector
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        experience = 0;
        UIController.Instance.UpdateExperienceSlider(experience, playerLevels[currentLevel]);

    }

    // Update is called once per frame
    void Update()
    {


        if (Time.timeScale > 0)
        {
            float directionX = Input.GetAxisRaw("Horizontal");
            float directionY = Input.GetAxisRaw("Vertical");

            animator.SetFloat("moveX", directionX);
            animator.SetFloat("moveY", directionY);

            playerDirection = new Vector2(directionX, directionY).normalized;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2"))
            {
                EnterBoost();
            }
            else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2"))
            {
                ExitBoost();

            }

            if (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Fire1"))
            {
                PhaserWeapon.Instance.Shoot();
            }
        }



    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed * boost, playerDirection.y * moveSpeed * boost);

        if (boosting)
        {
            if (energy >= 0.2) energy -= 0.2f;
            else
            {
                ExitBoost();
            }
        }
        else
        {
            if (energy < maxEnergy)
            {
                energy += energyRegen;
            }
        }
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);


    }

    private void EnterBoost()
    {
        if (energy > 10)
        {
            animator.SetBool("boosting", true);
            boost = boostPower;
            boosting = true;
            engineEffect.Play();

        }

    }

    public void ExitBoost()
    {
        animator.SetBool("boosting", false);
        boost = 1f;
        boosting = false;

    }

    // Collision với Obstacle được xử lý trong Asteroid.cs
    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Obstacle"))
    //     {
    //         TakeDamage(1);
    //     }
    // }

    public void TakeDamage(int damage)
    {
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);

        if (health <= 0)
        {
            // Player chết - không cần reset material
            spriteRenderer.material = whiteMaterial;
            boost = 0f;
            gameObject.SetActive(false);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            GameManager.Instance.GameOver();
        }
        else
        {
            // Player còn sống - flash white và reset material
            spriteRenderer.material = whiteMaterial;
            StartCoroutine("ResetMaterial");
        }
    }

    IEnumerator ResetMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }

    public void GetExperience(int exp)
    {
        experience += exp;
        UIController.Instance.UpdateExperienceSlider(experience, playerLevels[currentLevel]);
        if (experience > playerLevels[currentLevel])
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        experience -= playerLevels[currentLevel];
        if (currentLevel < maxLevel - 1) currentLevel++;
        UIController.Instance.UpdateExperienceSlider(experience, playerLevels[currentLevel]);
        PhaserWeapon.Instance.LevelUp();
        // Bỏ tăng health khi level up - chỉ upgrade vũ khí
        // maxHealth++; 
        // health = maxHealth;
        // UIController.Instance.UpdateHealthSlider(health, maxHealth);

    }
}
