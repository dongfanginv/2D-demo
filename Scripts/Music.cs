using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//音效管理类
public class Music : MonoBehaviour
{
    public AudioClip[] Bgm;
    public AudioClip[] button;
    [HideInInspector]
    public AudioSource AS;
    private void Awake()
    {
        AS = GetComponent<AudioSource>();
        string scene=SceneManager.GetActiveScene().name;
        if(scene == "MainMenu")
        {
            AS.clip=Bgm[0];
            AS.loop=true;
            AS.Play();
        }else if(scene == "Level1" || scene == "Level2")
        {
            AS.clip=Bgm[1];
            AS.loop = true;
            AS.Play();
        }
        else
        {
            AS.clip=Bgm[2]; AS.Play(); AS.loop = true;
        }
    }

}
