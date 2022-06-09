using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderDetector : MonoBehaviour
{
    public string targetTag = string.Empty;

    private void OnTriggerEnter(Collider other)
    { 
        if (other.gameObject.CompareTag(targetTag) == true)
        {
            gameObject.SendMessageUpwards("OnCkTarget", other.gameObject, SendMessageOptions.DontRequireReceiver);
        }
    }
}
