using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float radius = 5f;

    public float power = 200f;

    public float flyingSize = 3f;

    public float damage = 10f;

    public int coin = 100;

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

    Vector3 CurrentVelocitySpd = Vector3.zero;
    float VelocityChangeSpd = 0.1f;

    private CharacterController characterController = null;
    private Coroutine attackCoroutine = null;
    private Animator animator = null;

    GameObject nearobject;
    GameObject equipWeapon;
    int equipWeaponIndex = -1;

    public GameObject shopUI;
    public GameObject failedUI;
    public int[] itempPrice;
    public bool useShop;

    public Image[] staffImage;

    public Button[] button;

    public Text coinText;

    public void OpenShop()
    {
        coinText.text = string.Format("{0:#,###}", coin);
        useShop = true;
        Time.timeScale = 0;
        shopUI.SetActive(true);
    }

    public void OnClickExit()
    {
        useShop = false;
        Time.timeScale = 1;
        shopUI.SetActive(false);
        failedUI.SetActive(false);
    }

    public void OnClickFailedExit()
    {
        failedUI.SetActive(false);
    }

    public void Buy(int index)
    {
        int price = itempPrice[index];

        if (price > coin)
        {
            failedUI.SetActive(true);
            return;
        }
        if (index == 0)
        {
            if(hasWeapons[0]==false)
            {
                staffImage[index].color = new Color(0.3f, 0.3f, 0.3f, 1f);
                button[index].enabled = false;
                button[index].image.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                hasWeapons[0] = true;
                coin -= price;
                coinText.text = string.Format("{0:#,###}", coin);
            }
            return;
        }
        else if(index==1)
        {
            if (hasWeapons[1] == false)
            {
                staffImage[index].color = new Color(0.3f, 0.3f, 0.3f, 1f);
                button[index].enabled = false;
                button[index].image.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                hasWeapons[1] = true;
                coin -= price;
                coinText.text = string.Format("{0:#,###}", coin);
            }
            return;
        }
        else if (index == 2)
        {
            if (hasWeapons[2] == false)
            {
                staffImage[index].color = new Color(0.3f, 0.3f, 0.3f, 1f);
                button[index].enabled = false;
                button[index].image.color = new Color(0.3f, 0.3f, 0.3f, 1f);
                hasWeapons[2] = true;
                coin -= price;
                coinText.text = string.Format("{0:#,###}", coin);
            }
            return;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        Lhand.gameObject.SetActive(false);
        Rhand.gameObject.SetActive(false);
    }


    private void Update()
    {
        InputShop();
        if (!useShop)
        {
            GetInput();
            Swap();
            if (isMove)
                Move();
            AnimationUpdate();
        }
    }


    void InputShop()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!useShop)
                OpenShop();
            else
                OnClickExit();
        }
    }

    void GetInput()
    {
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

            GUILayout.Label("아이템 획득 : E, 사용 : R", labelStyle);

            GUILayout.Label("상점: TAB ", labelStyle);

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

}
