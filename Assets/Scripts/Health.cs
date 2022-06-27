using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Health : MonoBehaviour
{
    public Image healthBar;
    public GameObject endingUI;
    public SpawnMonster spawnMonster;
    public bool heal = false;
    public float health, maxHealth = 100;
    // Start is called before the first frame update
    void Start()
    {
        spawnMonster = GameObject.Find("SpawnMonster").GetComponent<SpawnMonster>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnMonster.round % 5 == 0 && !heal)
        {
            heal = true;
            health += 30;
        }

        if(spawnMonster.round%5==1)
            heal = false;

        if (health > maxHealth) 
            health = maxHealth;

        if (health <= 0)
            endingUI.SetActive(true);

        HealthBarFiller();
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = health / maxHealth;
    }

}
