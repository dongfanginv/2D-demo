using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//玩家触碰到宝石捡起
public class Stone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Canva mycanvas = GameObject.Find("/Canvas").GetComponent<Canva>();
            int life = PlayerPrefs.GetInt("playerstone") + 1;
            PlayerPrefs.SetInt("playerstone", life);
            mycanvas.updatestone();
            Destroy(this.gameObject);
        }

    }
}
