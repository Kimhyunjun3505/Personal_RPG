using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartButton : MonoBehaviour
{
    public GameObject setting;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���� ȭ�� ����
    /// </summary>
    public void Main()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
    /// <summary>
    /// ����â �ѱ�
    /// </summary>
    public void Setting()
    {
        setting.SetActive(true);
    }
    /// <summary>
    /// ����â ����
    /// </summary>
    public void Exit()
    {
        setting.SetActive(false);
    }
}
