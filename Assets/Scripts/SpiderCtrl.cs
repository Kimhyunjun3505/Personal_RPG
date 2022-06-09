using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpiderCtrl : MonoBehaviour
{
    //�Ź� ����
    public enum SpiderState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }

    //�Ź� �⺻ �Ӽ�
    [Header("�⺻ �Ӽ�")]
    //�Ź� �ʱ� ����
    public SpiderState skullState = SpiderState.None;
    //�Ź� �̵� �ӵ�
    public float spdMove = 1f;
    //�Ź̰� �� Ÿ��
    public GameObject targetCharactor = null;
    //�Ź̰� �� Ÿ�� ��ġ���� 
    public Transform targetTransform = null;
    //�Ź̰� �� Ÿ�� ��ġ
    public Vector3 posTarget = Vector3.zero;

    //�Ź� �ִϸ��̼� ������Ʈ ĳ�� 
    private Animation spiderAnimation = null;
    //�Ź� Ʈ������ ������Ʈ ĳ��
    private Transform spiderTransform = null;

    [Header("�ִϸ��̼� Ŭ��")]
    public AnimationClip IdleAnimClip = null;
    public AnimationClip MoveAnimClip = null;
    public AnimationClip AtkAnimClip = null;
    public AnimationClip DamageAnimClip = null;
    public AnimationClip DieAnimClip = null;

    [Header("�����Ӽ�")]
    //�Ź� ü��
    public int hp = 100;
    //�Ź� ���� �Ÿ�
    public float AtkRange = 1.5f;
    //�Ź� �ǰ� ����Ʈ
    public GameObject effectDamage = null;
    //�Ź� ���� ����Ʈ
    public GameObject effectDie = null;

    private SkinnedMeshRenderer skinnedMeshRenderer = null;


    void OnAtkAnmationFinished()
    {
        Debug.Log("Atk Animation finished");
    }

    void OnDmgAnmationFinished()
    {
        Debug.Log("Dmg Animation finished");
    }

    void OnDieAnmationFinished()
    {
        Debug.Log("Die Animation finished");

        Instantiate(effectDie, spiderTransform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    /// <summary>
    /// �ִϸ��̼� �̺�Ʈ�� �߰����ִ� ��. 
    /// </summary>
    /// <param name="clip">�ִϸ��̼� Ŭ�� </param>
    /// <param name="funcName">�Լ��� </param>
    void OnAnimationEvent(AnimationClip clip, string funcName)
    {
        AnimationEvent retEvent = new AnimationEvent();
        retEvent.functionName = funcName;
        retEvent.time = clip.length - 0.1f;
        clip.AddEvent(retEvent);
    }


    // Start is called before the first frame update
    void Start()
    {
        skullState = SpiderState.Idle;

        spiderAnimation = GetComponent<Animation>();
        spiderTransform = GetComponent<Transform>();

        spiderAnimation[IdleAnimClip.name].wrapMode = WrapMode.Loop;
        spiderAnimation[MoveAnimClip.name].wrapMode = WrapMode.Loop;
        spiderAnimation[AtkAnimClip.name].wrapMode = WrapMode.Once;
        spiderAnimation[DamageAnimClip.name].wrapMode = WrapMode.Once;

        spiderAnimation[DamageAnimClip.name].layer = 10;
        spiderAnimation[DieAnimClip.name].wrapMode = WrapMode.Once;
        spiderAnimation[DieAnimClip.name].layer = 10;

        OnAnimationEvent(AtkAnimClip, "OnAtkAnmationFinished");
        OnAnimationEvent(DamageAnimClip, "OnDmgAnmationFinished");
        OnAnimationEvent(DieAnimClip, "OnDieAnmationFinished");

        skinnedMeshRenderer = spiderTransform.Find("Polygonal Metalon").GetComponent<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// �Ź� ���¿� ���� ������ �����ϴ� �Լ� 
    /// </summary>
    void CkState()
    {
        switch (skullState)
        {
            case SpiderState.Idle:
                setIdle();
                break;
            case SpiderState.GoTarget:
            case SpiderState.Move:
                setMove();
                break;
            case SpiderState.Atk:
                setAtk();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CkState();
        AnimationCtrl();
    }

    /// <summary>
    /// �Ź� ���°� ��� �� �� ���� 
    /// </summary>
    void setIdle()
    {
        if (targetCharactor == null)
        {
            posTarget = new Vector3(spiderTransform.position.x + Random.Range(-10f, 10f),
                                    spiderTransform.position.y + 1000f,
                                    spiderTransform.position.z + Random.Range(-10f, 10f)
                );
            Ray ray = new Ray(posTarget, Vector3.down);
            RaycastHit infoRayCast = new RaycastHit();
            if (Physics.Raycast(ray, out infoRayCast, Mathf.Infinity) == true)
            {
                posTarget.y = infoRayCast.point.y;
            }
            skullState = SpiderState.Move;
        }
        else
        {
            skullState = SpiderState.GoTarget;
        }
    }

    /// <summary>
    /// �Ź� ���°� �̵� �� �� �� 
    /// </summary>
    void setMove()
    {
        Vector3 distance = Vector3.zero;
        Vector3 posLookAt = Vector3.zero;

        //�ذ� ����
        switch (skullState)
        {
            case SpiderState.Move:
                if (posTarget != Vector3.zero)
                {
                    distance = posTarget - spiderTransform.position;

                    if (distance.magnitude < AtkRange)
                    {
                        StartCoroutine(setWait());
                        return;
                    }
                    posLookAt = new Vector3(posTarget.x,
     
                                            spiderTransform.position.y,
                                            posTarget.z);
                }
                break;
            case SpiderState.GoTarget:
                if (targetCharactor != null)
                {
                    distance = targetCharactor.transform.position - spiderTransform.position;
                    if (distance.magnitude < AtkRange)
                    {
                        skullState = SpiderState.Atk;
                        return;
                    }
                    posLookAt = new Vector3(targetCharactor.transform.position.x,
                                            spiderTransform.position.y,
                                            targetCharactor.transform.position.z);
                }
                break;
            default:
                break;

        }
        Vector3 direction = distance.normalized;

        direction = new Vector3(direction.x, 0f, direction.z);

        Vector3 amount = direction * spdMove * Time.deltaTime;

        spiderTransform.Translate(amount, Space.World);

        spiderTransform.LookAt(posLookAt);
    }

    /// <summary>
    /// ��� ���� ���� �� 
    /// </summary>
    /// <returns></returns>
    IEnumerator setWait()
    {
        skullState = SpiderState.Wait;
        float timeWait = Random.Range(1f, 3f);
        yield return new WaitForSeconds(timeWait);
        skullState = SpiderState.Idle;
    }

    /// <summary>
    /// �ִϸ��̼��� ��������ִ� �� 
    /// </summary>
    void AnimationCtrl()
    {
        switch (skullState)
        {
            case SpiderState.Wait:
            case SpiderState.Idle:
                spiderAnimation.CrossFade(IdleAnimClip.name);
                break;
            case SpiderState.Move:
            case SpiderState.GoTarget:
                spiderAnimation.CrossFade(MoveAnimClip.name);
                break;
            case SpiderState.Atk:
                spiderAnimation.CrossFade(AtkAnimClip.name);
                break;
            case SpiderState.Die:
                spiderAnimation.CrossFade(DieAnimClip.name);
                break;
            default:
                break;

        }
    }

    ///<summary>
    ///�þ� ���� �ȿ� �ٸ� Trigger �Ǵ� ĳ���Ͱ� ������ ȣ�� �ȴ�.
    ///�Լ� ������ ��ǥ���� ������ ��ǥ���� �����ϰ� �Ź̸� Ÿ�� ��ġ�� �̵� ��Ų�� 
    ///</summary>

    void OnCkTarget(GameObject target)
    {
        targetCharactor = target;
        targetTransform = targetCharactor.transform;

        skullState = SpiderState.GoTarget;

    }

    /// <summary>
    /// �Ź� ���� ���� ���
    /// </summary>
    void setAtk()
    {
        float distance = Vector3.Distance(targetTransform.position, spiderTransform.position); 
        if (distance > AtkRange + 0.5f)
        {
            skullState = SpiderState.GoTarget;
        }
    }


    /// <summary>
    /// �Ź� �ǰ� �浹 ���� 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerAtk") == true)
        {
            hp -= 10;
            if (hp > 0)
            {
    
                Instantiate(effectDamage, other.transform.position, Quaternion.identity);

                spiderAnimation.CrossFade(DamageAnimClip.name);
                effectDamageTween();
            }
            else
            {
                skullState = SpiderState.Die;
            }
        }
    }

    /// <summary>
    /// �ǰݽ� ���� ������ ��½��½ ȿ���� �ش�
    /// </summary>
    void effectDamageTween()
    {
        Color colorTo = Color.red;

        skinnedMeshRenderer.material.DOColor(colorTo, 0f).OnComplete(OnDamageTweenFinished);
    }

    /// <summary>
    /// �ǰ�����Ʈ ����� �̺�Ʈ �Լ� ȣ��
    /// </summary>
    void OnDamageTweenFinished()
    {
        skinnedMeshRenderer.material.DOColor(Color.white, 2f);
    }
}
