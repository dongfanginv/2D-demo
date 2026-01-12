using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//放置在角色脚下的collider，用于检测和地面等的接触

public class Playerbutton : MonoBehaviour
{
    // Start is called before the first frame update
    Player playerScript;
    private void Awake()
    {
        playerScript=GetComponentInParent<Player>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    //触碰到地面或者平台
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            playerScript.canjump = true;
            playerScript.jumping=false;
            playerScript.myAnim.SetBool("Jump",false);
        }
        if (collision.tag == "AirPlat")
        {
            playerScript.canjump = true;
            playerScript.jumping = false;
            playerScript.myAnim.SetBool("Jump", false);
            playerScript.transform.parent = collision.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "AirPlat")
        {
            try
            {
                playerScript.transform.parent = null;
            }
            catch(NullReferenceException)
            {
                return;
            }
        }
    }
}
