using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//初始化玩家相关属性
public class FirstPlay : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        PlayerPrefs.SetInt("playerlife", 5);
        PlayerPrefs.SetInt("playerstone", 0);
    }
}
