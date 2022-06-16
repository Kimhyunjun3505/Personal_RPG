using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceStaff : MonoBehaviour
{
    public GameObject effectDamage = null;
    Vector3 pos;
    public bool isUse = false;

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
            if (Input.GetKeyDown(KeyCode.R))
            {
                pos = new Vector3(transform.position.x, 0, transform.position.z);
                Instantiate(effectDamage, pos, Quaternion.identity);
            }
        }
    }
}
