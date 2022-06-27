using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    /// <summary>
    /// ÄÚÀÎ ¸Ô±â
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().coin += 10000;
            Destroy(gameObject);
        }
    }
}
