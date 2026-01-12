using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
//×ª³¡¶¯»­
public class FadeInOut : MonoBehaviour
{
   public static FadeInOut instance;
    public GameObject fadeinout;
    public Animator anim;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else 
        { 
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void SetFadeInOut(string levelname)
    {
        StartCoroutine(FadeaAction(levelname));
    }
    IEnumerator FadeaAction(string levelname)
    {
        fadeinout.SetActive(true);
        yield return new WaitForSecondsRealtime(1.5f);
        anim.Play("FadeOut");
        
        SceneManager.LoadScene(levelname);
        yield return new WaitForSecondsRealtime(1.5f);
        fadeinout.SetActive(false);
        Time.timeScale = 1.0f; 
    }
}
