using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceStaff : MonoBehaviour
{
    public GameObject effectDamage = null;
    Vector3 pos;
    public bool isUse = false;

    private float coolTime=1;
    public Image img_Skill;

    private void OnEnable()
    {
        isUse = true;
    }

    private void OnDisable()
    {
        isUse = false;
    }

    private void Update()
    {
        if (isUse)
        {
            if (Input.GetKeyDown(KeyCode.R)&&coolTime<=1)
            {
                pos = new Vector3(transform.position.x, 0, transform.position.z);
                Instantiate(effectDamage, pos, Quaternion.identity);
                StartCoroutine(CoolTime(5f));
            }
        }
    }

    IEnumerator CoolTime(float cool) 
    {
        coolTime = cool;
        while (coolTime > 1.0f) 
        { 
            coolTime -= Time.deltaTime; 
            img_Skill.fillAmount = (1.0f / coolTime); 
            yield return new WaitForFixedUpdate(); 
        } 
      
    }
}
