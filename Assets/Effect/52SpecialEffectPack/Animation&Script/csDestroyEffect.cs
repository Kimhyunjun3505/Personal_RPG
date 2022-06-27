using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour {

    private void Start()
    {
        StartCoroutine(DestroySkill());
    }

    /// <summary>
    /// 스킬 삭제
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroySkill()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
