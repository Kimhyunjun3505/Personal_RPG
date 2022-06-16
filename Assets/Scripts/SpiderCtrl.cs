using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpiderCtrl : MonoBehaviour
{
    public float maxHealth = 50;
    public float curHealth;
    public Transform target;

    Rigidbody rb;
    BoxCollider boxCollider;
    Material mat;
    CharacterMove player;

    private void Awake()
    {
        player = GameObject.Find("Golem").GetComponent<CharacterMove>();
        curHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="PlayerAtk")
        {
            curHealth -= player.damage;
            StartCoroutine(OnDamage());
        }
        if(other.CompareTag("IceAge"))
        {
            mat.color = Color.blue;
            StartCoroutine(OnIce());
        }
    }

    IEnumerator OnDamage()
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth>0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.green;
            gameObject.layer = 6;
            Destroy(gameObject, 4);
        }
    }

    IEnumerator OnIce()
    {
        mat.color = Color.blue;
        yield return new WaitForSeconds(3f);
    }
}
