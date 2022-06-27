using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireStaff : MonoBehaviour
{
    public GameObject effectDamage = null;
    Vector3 pos;
    public bool isUse = false;

    public CoolTimeUI coolTimeUi;

    public Image img_Skill;
    /// <summary>
    /// �ش� ������ ���
    /// </summary>
    private void OnEnable()
    {
        img_Skill.gameObject.SetActive(true);
        img_Skill.color = new Color(1, 1, 1, 1f);
        isUse = true;
    }
    /// <summary>
    /// �ش� ������ ���� �ٸ� ������ ��ü
    /// </summary>
    private void OnDisable()
    {
        if (img_Skill != null)
        {
            img_Skill.color = new Color(1, 1, 1, 0f);
        }
        isUse = false;
    }

    private void Update()
    {
        if (isUse)
        {
            if (Input.GetKeyDown(KeyCode.R) && coolTimeUi.fireCoolTime <= 1)
            {
                pos = new Vector3(transform.position.x, 0, transform.position.z);
                Instantiate(effectDamage, pos, Quaternion.identity);
                coolTimeUi.FireAge();
            }
        }
    }
}
