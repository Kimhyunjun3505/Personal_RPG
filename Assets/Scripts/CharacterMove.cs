using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float radius = 5f;

    public float power = 200f;

    public float flyingSize = 3f;

    public float damage = 10f;

    public GameObject[] weapons;
    public bool[] hasWeapons;

    public AnimationClip attackClip;

    public Collider Lhand;
    public Collider Rhand;

    public Transform posRSkillEffect;
    public Transform posLSkillEffect;
    public IceStaff iceStaff;

    public float speed = 2f;
    public float runSpeed = 10f;
    public float bodyRotateSpeed = 100f;
    public float minusPosY = 0.5f;

    bool isAttack = false;
    bool isWalk = false;
    bool isRun = false;
    bool isMove = true;
    bool isSwap = false;
    bool isJumpAttack = false;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;

    public GameObject effectDamage = null;

    Vector3 CurrentVelocitySpd = Vector3.zero;
    float VelocityChangeSpd = 0.1f;

    private CharacterController characterController = null;
    private Coroutine attackCoroutine = null;
    private Animator animator = null;

    GameObject nearobject;
    GameObject equipWeapon;
    int equipWeaponIndex = -1;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        //iceStaff = GameObject.Find("Staff_02 (1)").GetComponent<IceStaff>();
        characterController = GetComponent<CharacterController>();
        Lhand.gameObject.SetActive(false);
        Rhand.gameObject.SetActive(false);
    }


    private void Update()
    {

        GetInput();
        Interation();
        Swap();
        if (isMove)
            Move();
        AnimationUpdate();
    }


    void GetInput()
    {
        iDown = Input.GetButtonDown("Interation");
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        if(iceStaff.isUse)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                animator.SetTrigger("Skill");
            }
        }
        if (Input.GetMouseButtonDown(0)&&!isJumpAttack)
        {
            AttackAnimationStart();
        }
    }
    
    void JumpAtkOn()
    {
        isJumpAttack = true;
    }

    void JumpAtkOff()
    {
        isJumpAttack = false;

    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Swap()
    {
        if (sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0))
            return;
        if (sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;


        int weaponIndex = -1;

        if (sDown1)
            weaponIndex = 0;
        if (sDown2)
            weaponIndex = 1;
        if (sDown3)
            weaponIndex = 2;

        if ((sDown1||sDown2||sDown3)&&!isAttack&&!isSwap)
        {
            if(equipWeapon!=null)
                equipWeapon.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);
            animator.SetTrigger("Swap");
            isSwap = true;

            Invoke("SwapOut", 0.3f);
        }

    }

    void Interation()
    {
        if (iDown && nearobject != null)
        {
            if (nearobject.tag == "Weapon")
            {
                Item item = nearobject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                Destroy(nearobject);
            }
        }

    }

    private IEnumerator IsAttackFalse()
    {
        yield return new WaitForSeconds(attackClip.length-1.5f);
        isAttack = false;
        characterController.enabled = true;
    }

    private void AttackAnimationStart()
    {
        isAttack = true;
        characterController.enabled = false;
        animator.SetTrigger("Attack");
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(IsAttackFalse());

        Collider[] Rcolliders = Physics.OverlapSphere(posRSkillEffect.position, radius);
        Collider[] Lcolliders = Physics.OverlapSphere(posLSkillEffect.position, radius);

        foreach (Collider collider in Rcolliders)
        {
            if (collider.gameObject.CompareTag("PlayerAtk") == true || collider.gameObject.CompareTag("MonsterAtk") == true)
            {
                continue;
            }

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
        if (dir != Vector3.zero)
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
        Vector3 right = new Vector3(forward.z, 0f, -forward.x);

        Vector3 amount = h * right + v * forward;

        if (!isAttack)
        {
            if (isSwap)
                amount = Vector3.zero;
            characterController.Move(amount * speed * Time.deltaTime);
        }

        if (GetVelocitySpd() > 0.1)
            isWalk = true;
        else
            isWalk = false;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
            isRun = true;
        }

        if(!isJumpAttack)
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

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Weapon")
            nearobject = other.gameObject;
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon")
            nearobject = null;
    }
}
