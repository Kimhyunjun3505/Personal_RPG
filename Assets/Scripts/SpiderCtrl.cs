using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpiderCtrl : MonoBehaviour
{
    //거미 상태
    public enum SpiderState { None, Idle, Move, Wait, GoTarget, Atk, Damage, Die }

    //거미 기본 속성
    [Header("기본 속성")]
    //거미 초기 상태
    public SpiderState skullState = SpiderState.None;
    //거미 이동 속도
    public float spdMove = 1f;
    //거미가 본 타겟
    public GameObject targetCharactor = null;
    //거미가 본 타겟 위치정보 
    public Transform targetTransform = null;
    //거미가 본 타겟 위치
    public Vector3 posTarget = Vector3.zero;

    //거미 애니메이션 컴포넌트 캐싱 
    private Animation spiderAnimation = null;
    //거미 트랜스폼 컴포넌트 캐싱
    private Transform spiderTransform = null;

    [Header("애니메이션 클립")]
    public AnimationClip IdleAnimClip = null;
    public AnimationClip MoveAnimClip = null;
    public AnimationClip AtkAnimClip = null;
    public AnimationClip DamageAnimClip = null;
    public AnimationClip DieAnimClip = null;

    [Header("전투속성")]
    //거미 체력
    public int hp = 100;
    //거미 공격 거리
    public float AtkRange = 1.5f;
    //거미 피격 이펙트
    public GameObject effectDamage = null;
    //거미 다이 이펙트
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
    /// 애니메이션 이벤트를 추가해주는 함. 
    /// </summary>
    /// <param name="clip">애니메이션 클립 </param>
    /// <param name="funcName">함수명 </param>
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
    /// 거미 상태에 따라 동작을 제어하는 함수 
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
    /// 거미 상태가 대기 일 때 동작 
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
    /// 거미 상태가 이동 일 때 동 
    /// </summary>
    void setMove()
    {
        Vector3 distance = Vector3.zero;
        Vector3 posLookAt = Vector3.zero;

        //해골 상태
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
    /// 대기 상태 동작 함 
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
    /// 애니메이션을 재생시켜주는 함 
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
    ///시야 범위 안에 다른 Trigger 또는 캐릭터가 들어오면 호출 된다.
    ///함수 동작은 목표물이 들어오면 목표물을 설정하고 거미를 타겟 위치로 이동 시킨다 
    ///</summary>

    void OnCkTarget(GameObject target)
    {
        targetCharactor = target;
        targetTransform = targetCharactor.transform;

        skullState = SpiderState.GoTarget;

    }

    /// <summary>
    /// 거미 상태 공격 모드
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
    /// 거미 피격 충돌 검출 
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
    /// 피격시 몬스터 몸에서 번쩍번쩍 효과를 준다
    /// </summary>
    void effectDamageTween()
    {
        Color colorTo = Color.red;

        skinnedMeshRenderer.material.DOColor(colorTo, 0f).OnComplete(OnDamageTweenFinished);
    }

    /// <summary>
    /// 피격이펙트 종료시 이벤트 함수 호출
    /// </summary>
    void OnDamageTweenFinished()
    {
        skinnedMeshRenderer.material.DOColor(Color.white, 2f);
    }
}
