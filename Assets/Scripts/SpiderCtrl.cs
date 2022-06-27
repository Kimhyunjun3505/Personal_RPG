using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;

public class SpiderCtrl : MonoBehaviour
{
    public float maxHealth = 50;
    public float curHealth;
    public Transform target;
    public bool isChase;
    public bool isDie = false;
    public bool isIce = false;
    public bool isFire = false;
    public bool isPoison = false;
    public bool isAtk = false;
    public GameObject effectDamage = null;
    public GameObject damagedEffect = null;
    public GameObject coin = null;

    public Collider col;

    Rigidbody rb;
    BoxCollider boxCollider;
    Material mat;
    Player player;
    NavMeshAgent nav;
    Animator animator;

    private void Awake()
    {
        target = GameObject.Find("Target_Tree").transform;
        player = GameObject.Find("Golem").GetComponent<Player>();
        curHealth = maxHealth;
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        //Debug.Log(nav.speed);
        Invoke("ChaseStart", 1);
    }

    /// <summary>
    /// 적한테 가기
    /// </summary>
    private void ChaseStart()
    {
        isChase = true;
        animator.SetBool("IsWalk", true);
    }

    private void Update()
    {
        if(isPoison)
            nav.speed = 2f;
        else
            nav.speed = 3.5f;
        if (isChase && !isIce && transform.position.y <= 1 && !isDie)
        {
            nav.SetDestination(target.position);
            animator.SetBool("IsWalk", true);
        }
    }

    /// <summary>
    /// 가속도 0
    /// </summary>
    private void FreezeVelocity()
    {
        if (isChase)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            if (!isDie && !isIce)
                Invoke("ChaseStart", 3);
        }
    }

    private void FixedUpdate()
    {
        FreezeVelocity();
    }

    /// <summary>
    /// 공격 시작 함수
    /// </summary>
    public void SetTrueAtk()
    {
        Instantiate(effectDamage, col.transform.position, Quaternion.identity);
        col.gameObject.SetActive(false);
        target.GetComponent<Health>().health -= 1;
    }
    /// <summary>
    /// 공격 종료
    /// </summary>
    public void SetFalseAtk()
    {
        animator.SetBool("IsAttack", false);
        StartCoroutine(colActive());
    }
    /// <summary>
    /// 공격 콜라이더 키기
    /// </summary>
    /// <returns></returns>
    IEnumerator colActive()
    {
        yield return new WaitForSeconds(0.2f);
        col.gameObject.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {

        if(other.tag == "PoisonAge")
        {
            isPoison = true;
            curHealth -= 5f;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }

        if (col.gameObject.activeSelf == true)
        {
            if (other.tag == "Target")
            {
                animator.SetBool("IsAttack", true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag=="PlayerAtk")
        {
            isChase = false;
            curHealth -= player.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
        if (other.tag == "FireAge")
        {
            isChase = false;
            curHealth -= 100;
            isFire = true;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
        }
        if (other.CompareTag("IceAge"))
        {
            mat.color = Color.blue;
            StartCoroutine(OnIce());
        }
    }

    /// <summary>
    /// 데미지를 받았을때 실행하는 함수
    /// </summary>
    /// <param name="reactVec"></param>
    /// <returns></returns>
    IEnumerator OnDamage(Vector3 reactVec)
    {
        if(isIce)
        {
            nav.enabled = true;
            isIce = false;
        }

        isChase = false;
        mat.color = Color.red;
        reactVec = reactVec.normalized;
        rb.freezeRotation = false;
        yield return new WaitForSeconds(0.1f);
        if(!isFire&&!isPoison)
        {
            Instantiate(damagedEffect, transform.position, Quaternion.identity);
            rb.AddForce(reactVec * 1000, ForceMode.Force);
        }
        yield return new WaitForSeconds(0.5f);


        if (!isIce)
            isChase = true;

        if (isFire)
            isFire = false;

        if (curHealth>0)
        {
            if (!isIce)
                mat.color = Color.white;
        }
        else
        {
            isChase = false;
            isDie = true;
            nav.enabled = false;
            animator.SetTrigger("DoDie");
            mat.color = Color.green;
            gameObject.layer = 6;
            Instantiate(coin, transform.position, Quaternion.identity);
            Destroy(gameObject, 1);
        }
    }

    /// <summary>
    /// 동상 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator OnIce()
    {
        mat.color = Color.blue;
        nav.enabled = false;
        isChase = false;
        isIce = true;
        animator.SetBool("IsWalk", false);
        animator.SetBool("IsAttack", false);
        yield return new WaitForSeconds(3f);
        if(isIce)
        {
            nav.enabled = true;
            mat.color = Color.white;
            isIce = false;
            isChase = true;
        }
    }
}
