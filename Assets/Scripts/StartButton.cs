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
    /// 메인 화면 가기
    /// </summary>
    public void Main()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
    /// <summary>
    /// 세팅창 켜기
    /// </summary>
    public void Setting()
    {
        setting.SetActive(true);
    }
    /// <summary>
    /// 세팅창 끄기
    /// </summary>
    public void Exit()
    {
        setting.SetActive(false);
    }
}
