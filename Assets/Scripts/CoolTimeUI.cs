using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolTimeUI : MonoBehaviour
{
    public Image ice_Skill;
    public Image fire_Skill;
    public Image poison_Skill;
    public float iceCoolTime = 1;
    public float fireCoolTime = 1;
    public float poisonCoolTime = 1;

    public void IceAge()
    {
        StartCoroutine(IceCoolTime(5f));
    }

    public void FireAge()
    {
        StartCoroutine(FireCoolTime(10f));
    }

    public void PoisonAge()
    {
        StartCoroutine(PoisonCoolTime(10f));
    }

    /// <summary>
    /// 아이스에이지 쿨타임
    /// </summary>
    /// <param name="cool"></param>
    /// <returns></returns>
    IEnumerator IceCoolTime(float cool)
    {
        iceCoolTime = cool;
        while (iceCoolTime > 1.0f)
        {
            iceCoolTime -= Time.deltaTime;
            ice_Skill.fillAmount = (1.0f / iceCoolTime);
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// 파이어에이지 쿨타임
    /// </summary>
    /// <param name="cool"></param>
    /// <returns></returns>
    IEnumerator FireCoolTime(float cool)
    {
        fireCoolTime = cool;
        while (fireCoolTime > 1.0f)
        {
            fireCoolTime -= Time.deltaTime;
            fire_Skill.fillAmount = (1.0f / fireCoolTime);
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// 포이즌에이지 쿨타임
    /// </summary>
    /// <param name="cool"></param>
    /// <returns></returns>
    IEnumerator PoisonCoolTime(float cool)
    {
        poisonCoolTime = cool;
        while (poisonCoolTime > 1.0f)
        {
            poisonCoolTime -= Time.deltaTime;
            poison_Skill.fillAmount = (1.0f / poisonCoolTime);
            yield return new WaitForFixedUpdate();
        }
    }
}
