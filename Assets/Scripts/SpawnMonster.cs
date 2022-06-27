using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SpawnMonster : MonoBehaviour
{
    public GameObject monsterSpawner = null;

    public List<GameObject> monsters = new List<GameObject>();

    public int spawnMaxCnt = 50;

    public int monstersCount = 0;

    private int n = 1;

    public int round = 1;

    float rndPos = 230f;

    public bool respawn = true;

    public bool roundEnd = false;

    public float spawnDelay = 2f;

    public Text roundText;

    private void Start()
    {
        respawn = true;
        roundText.text = "Round: " + round;
    }

    private void Update()
    {
        if(respawn)
        {
            respawn = false;
            //나중에 20초로
            InvokeRepeating("Spawn", 5, spawnDelay);
        }
        else if(roundEnd)
        {
            monstersCount = 0;
            spawnMaxCnt += 2 * n;
            n++;
            round++;
            roundText.text = "Round: " + round;
            roundEnd = false;
            respawn = true;
            CancelInvoke("Spawn");
        }
    }
    /// <summary>
    /// 몬스터 스폰 함수
    /// </summary>
    public void Spawn()
    {

        if(monstersCount>=spawnMaxCnt)
        {
            roundEnd = true;
        }
        else
        {
            Vector3 vecSpawn = new Vector3(Random.Range(50, rndPos), 1000f, Random.Range(50, rndPos));

            if (vecSpawn.x > 85 && vecSpawn.x < 216 && vecSpawn.z > 69 && vecSpawn.z < 226)
            {
                while (true)
                {
                    vecSpawn = new Vector3(Random.Range(50, rndPos), 1000f, Random.Range(50, rndPos));
                    if ((vecSpawn.x < 85 || vecSpawn.x > 216 || vecSpawn.z < 69 || vecSpawn.z > 226))
                        break;
                }
            }

            Ray ray = new Ray(vecSpawn, Vector3.down);

            RaycastHit raycastHit = new RaycastHit();

            if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity) == true && raycastHit.point.y == 0)
            {
                vecSpawn.y = raycastHit.point.y;
            }

            GameObject newMonster = Instantiate(monsterSpawner, vecSpawn, Quaternion.identity);
            monstersCount++;
            monsters.Add(newMonster);
        }
    }
}
