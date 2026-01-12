using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//通关关卡的检测类
public class Wintrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            int chooselevel = PlayerPrefs.GetInt("chooseLevel");
            
            //string levelname=SceneManager.GetActiveScene().name;
            //string temp= levelname.Substring(5);
            int levelnumber=chooselevel+1;
            PlayerPrefs.SetInt("chooseLevel", levelnumber);
            string scenelevel="Level"+levelnumber;
            FadeInOut.instance.SetFadeInOut(scenelevel);

        }
    }
}
