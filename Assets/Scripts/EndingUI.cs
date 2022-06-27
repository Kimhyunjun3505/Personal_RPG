using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndingUI : MonoBehaviour
{
    public int round;
    public Text roundText;

    /// <summary>
    /// ���� ǥ��
    /// </summary>
    private void OnEnable()
    {
        round = GameObject.Find("SpawnMonster").GetComponent<SpawnMonster>().round;
        Time.timeScale = 0;
        roundText.text = "Round: " + round;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
