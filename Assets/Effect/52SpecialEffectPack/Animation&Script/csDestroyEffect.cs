using UnityEngine;
using System.Collections;

public class csDestroyEffect : MonoBehaviour {

    private void Start()
    {
        StartCoroutine(DestroyIceAge());
    }

    IEnumerator DestroyIceAge()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
