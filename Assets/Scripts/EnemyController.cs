using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public float health, startHealth;
    public Image healthbar;
    public bool isAlive = true;
    public ParticleSystem deathPoof;
    public GameObject healthbarGo, enemyGo;

    public void TakeDamage(float amount)
    {
        health -= amount;
        healthbar.fillAmount = health/startHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        deathPoof.Play();
        healthbarGo.SetActive(false);
        enemyGo.SetActive(false);
        StartCoroutine(DestroyEnemy());
    }

    IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
