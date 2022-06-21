using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMonster : MonoBehaviour
{
    public GameObject monsterSpawner = null;

    public List<GameObject> monsters = new List<GameObject>();

    public int spawnMaxCnt = 50;

    float rndPos = 250f;

    

    IEnumerator Spawn()
    {
        while(true)
        {
            if (monsters.Count > spawnMaxCnt)
            {
                break;
            }

            Vector3 vecSpawn = new Vector3(Random.Range(50, rndPos), 1000f, Random.Range(50, rndPos));

            Ray ray = new Ray(vecSpawn, Vector3.down);

            RaycastHit raycastHit = new RaycastHit();

            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true&&raycastHit.point.y==0)
            {
                vecSpawn.y = raycastHit.point.y;
            }

            GameObject newMonster = Instantiate(monsterSpawner, vecSpawn, Quaternion.identity);
            monsters.Add(newMonster);

            yield return new WaitForSeconds(5f);
        }
    }
       
    private void Start()
    {
        StartCoroutine("Spawn");
    }
}
