using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float radius = 5f;

    public float power = 200f;

    public float flyingSize = 3f;


    public Collider Lhand;
    public Collider Rhand;

    public Transform posRSkillEffect;
    public Transform posLSkillEffect;

    public float speed = 2f;
    public float runSpeed = 10f;
    public float bodyRotateSpeed = 100f;

    bool isWalk = false;
    bool isRun = false;
    bool isMove = true;

    public GameObject effectDamage = null;

    Vector3 CurrentVelocitySpd = Vector3.zero;
    float VelocityChangeSpd = 0.1f;

    private CharacterController characterController = null;
    private Animator animator = null;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Lhand.gameObject.SetActive(false);
        Rhand.gameObject.SetActive(false);
    }

    private void Update()
    {

        if (isMove)
            Move();

        if (Input.GetMouseButtonDown(0))
        {
            AttackAnimationStart();
        }

        AnimationUpdate();
    }

    private void AttackAnimationStart()
    {

        animator.SetTrigger("Attack");

        Collider[] Rcolliders = Physics.OverlapSphere(posRSkillEffect.position, radius);
        Collider[] Lcolliders = Physics.OverlapSphere(posLSkillEffect.position, radius);

        foreach (Collider collider in Rcolliders)
        {
            if (collider.gameObject.CompareTag("PlayerAtk") == true|| collider.gameObject.CompareTag("MonsterAtk") == true)
            {
                continue;
            }

            //Rigidbody component 존재 여부
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(power, posRSkillEffect.position, radius, flyingSize);
            }
        }

        foreach (Collider collider in Lcolliders)
        {
            if (collider.gameObject.CompareTag("PlayerAtk") == true || collider.gameObject.CompareTag("MonsterAtk") == true)
            {
                continue;
            }

            //Rigidbody component 존재 여부
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(power, posLSkillEffect.position, radius, flyingSize);
            }
        }
    }

    public void IsMoveTrue()
    {
        Lhand.gameObject.SetActive(false);
        Rhand.gameObject.SetActive(false);
        isMove = true;
    }
    public void IsMoveFalse()
    {
        Lhand.gameObject.SetActive(true);
        Rhand.gameObject.SetActive(true);
        isMove = false;
    }


    private void AnimationUpdate()
    {
        if (isWalk)
            animator.SetBool("Walk", true);
        else
            animator.SetBool("Walk", false);

        if (isRun)
            animator.SetBool("Run", true);
        else
            animator.SetBool("Run", false);

    }

    private void BodyRotate(Vector3 dir)
    {
        if(dir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), bodyRotateSpeed * Time.deltaTime);
    }

    private void OnGUI()
    {
        if (characterController != null)
        {
            var labelStyle = new GUIStyle();
            labelStyle.fontSize = 50;
            labelStyle.normal.textColor = Color.white;

            GUILayout.Label("MOVE : " + isMove.ToString(), labelStyle);

            GUILayout.Label("RUN : " + isRun.ToString(), labelStyle);

        }
    }

    void Move()
    {
        isRun = false;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = Camera.main.transform.TransformDirection(Vector3.forward);
        forward.y = 0f;
        Vector3 right = new Vector3(forward.z, 0f , -forward.x);

        Vector3 amount = h * right + v * forward;

        characterController.Move(amount * speed * Time.deltaTime);

        if (GetVelocitySpd() > 0)
            isWalk = true;
        else
            isWalk = false;

        if (Input.GetKey(KeyCode.E))
        {
            speed = runSpeed;
            isRun = true;
        }


        BodyRotate(amount);
    }

    float GetVelocitySpd()
    {
        if (characterController.velocity == Vector3.zero)
        {
            CurrentVelocitySpd = Vector3.zero;
        }
        else
        {
            Vector3 retVelocity = characterController.velocity;
            retVelocity.y = 0;
            CurrentVelocitySpd = Vector3.Lerp(
                CurrentVelocitySpd, retVelocity, VelocityChangeSpd * Time.fixedDeltaTime);
        }
        return CurrentVelocitySpd.magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MonsterAtk") == true)
        {
            Instantiate(effectDamage, other.transform.position, Quaternion.identity);
        }
    }
}
