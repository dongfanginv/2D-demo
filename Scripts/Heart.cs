using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//玩家触碰到Item增加血量
public class Heart : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Canva mycanvas = GameObject.Find("/Canvas").GetComponent<Canva>();
            int life = PlayerPrefs.GetInt("playerlife")+1;
            PlayerPrefs.SetInt("playerlife",life);
            mycanvas.updatelife();
            Destroy(this.gameObject);
        }
       
    }
    
}
