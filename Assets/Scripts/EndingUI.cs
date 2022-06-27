using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndingUI : MonoBehaviour
{
    public int round;
    public Text roundText;

    /// <summary>
    /// 라운드 표시
    /// </summary>
    private void OnEnable()
    {
        round = GameObject.Find("SpawnMonster").GetComponent<SpawnMonster>().round;
        Time.timeScale = 0;
        roundText.text = "Round: " + round;
    }

    /// <summary>
    /// 게임 종료
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
