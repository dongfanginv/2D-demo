using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//更新血量和宝石的显示
public class Canva : MonoBehaviour
{
    Text lifetext,stonetext;
    private void Awake()
    {

        updatelife();
        updatestone();
        
        
    }
    public void updatelife()
    {
        GameObject life = GameObject.Find("Canvas/LifeText");
        lifetext = life.GetComponent<Text>();
        lifetext.text = "x" + PlayerPrefs.GetInt("playerlife").ToString();
    }
    public void updatestone()
    {
        GameObject stone = GameObject.Find("Canvas/Itemtext");
        stonetext = stone.GetComponent<Text>();
        stonetext.text = "x" + PlayerPrefs.GetInt("playerstone").ToString();
    }
}
