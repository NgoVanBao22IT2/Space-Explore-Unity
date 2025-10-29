using UnityEngine;
using System.Collections.Generic;


public class PhaserBullet : MonoBehaviour
{
    PhaserWeapon weapon;

    void Start()
    {
        weapon = PhaserWeapon.Instance;
    }
    void Update()
    {
        transform.position += new Vector3(weapon.stats[weapon.weaponLevel].speed * Time.deltaTime, 0f);
        if (transform.position.x > 9)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid != null && weapon != null && weapon.stats != null && weapon.weaponLevel < weapon.stats.Count)
            {
                asteroid.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
            }
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.CompareTag("Critter"))
        {
            gameObject.SetActive(false);
        }
    }
}
