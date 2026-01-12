using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//菜单栏按钮的相关逻辑
public class Buttonscipt : MonoBehaviour
{
   
    public GameObject selectpanel,stopbutton,menubutton,repalybutton;
    Animator anim;
    GameObject myplay;
    Music music;
    private void Awake()
    {
         music = GameObject.Find("Music").GetComponent<Music>();
    }

    public void Replay()
    {
        string level=SceneManager.GetActiveScene().name;
        music.AS.PlayOneShot(music.button[0]);
        FadeInOut.instance.SetFadeInOut(level);
        Time.timeScale = 1f;
    }
    public void Mainmenu()
    {

        FadeInOut.instance.SetFadeInOut("MainMenu");
        music.AS.PlayOneShot(music.button[0]);
        Time.timeScale = 1f;
    }
    public void SelectOn()
    {
        selectpanel.SetActive(true);
        stopbutton.SetActive(false);
        music.AS.PlayOneShot(music.button[0]);
        Time.timeScale = 0f;
    }
    public void SelectOff()
    {
        selectpanel.SetActive(false);
        stopbutton.SetActive(true);
        Time.timeScale = 1f;
    }
    public void Mainmenuplay()
    {
        PlayerPrefs.SetInt("chooseLevel", 1);
        myplay = GameObject.Find("Idle");
        
        anim = myplay.GetComponent<Animator>();

        anim.SetBool("Run",true);
        GameObject playbutton = GameObject.Find("Canvas/Play");
        playbutton.SetActive(false);
        
        music.AS.PlayOneShot(music.button[0]);
        StartCoroutine(scenebegin());
        

    }
    IEnumerator scenebegin()
    {
        yield return new WaitForSeconds(3.0f);
        FadeInOut.instance.SetFadeInOut("Level1");
    }
}
