using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Health : MonoBehaviour
{
    public Image healthBar;

    public float health, maxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > maxHealth) 
            health = maxHealth;

        if(health<=0)
            SceneManager.LoadScene("GameOver");

        HealthBarFiller();
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    //public void Heal(float healingPoints)
    //{
    //    if (health < maxHealth)
    //        health += healingPoints;
    //}

    //public void Damage(float damagePoints)
    //{
    //    if (health > 0)
    //        health -= damagePoints;
    //}
}
