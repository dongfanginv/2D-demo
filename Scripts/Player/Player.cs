using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject attackCollider,boss;
    [HideInInspector]
    public Animator myAnim;
    Rigidbody2D myRigi;
    SpriteRenderer mySR;
    float jumpforce=20;
    AudioSource myAS;
    [SerializeField]
    AudioClip[] myAC;
    [HideInInspector]
    public bool isjump, canjump,isattack,jumping,ishurt,canhurt;
    int playerlife;
    Canva mycanvas;
    
    private void Awake()
    {
        mycanvas=GameObject.Find("/Canvas").GetComponent<Canva>();
        myAnim = GetComponent<Animator>();
        myRigi = GetComponent<Rigidbody2D>();
        mySR = GetComponent<SpriteRenderer>();
        myAS = GetComponent<AudioSource>();
        
        isjump = false;
        canjump = true;
        isattack = false;
        jumping = false;
        ishurt = false;
        canhurt = true;
        
        playerlife = PlayerPrefs.GetInt("playerlife");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    //控制角色的相关触发bool值
    void Update()
    {
        playerlife = PlayerPrefs.GetInt("playerlife");
        if (Input.GetKeyDown(KeyCode.Space) && canjump==true && !ishurt )
        {
            isjump = true;
            canjump=false;
            jumping = true;
        }
        if (Input.GetKeyDown(KeyCode.J) && !ishurt)
        {
            myAnim.SetTrigger("Attack");
            isattack = true;
            canjump=false;
        }
    }
    //控制角色移动，跳跃，rigidbody组件
    private void FixedUpdate()
    {
        float a = Input.GetAxisRaw("Horizontal");
        if ((isattack && !jumping) || ishurt)
        {
            a = 0;
        }
        if (a < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (a > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        myAnim.SetFloat("Run", Mathf.Abs(a));
        if (isjump)
        {
            myRigi.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
            isjump = false;
            myAnim.SetBool("Jump",true);
        }
        if (!ishurt)
        {
            myRigi.velocity = new Vector2(a * 5, myRigi.velocity.y);
        }
        
  
    }
    //掉下悬崖，碰撞到底部collider死亡
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name== "Bounddown")
        {
            playerlife = 0;
            myAnim.SetBool("Die", true);
            myAS.PlayOneShot(myAC[3]);
            myRigi.velocity = new Vector2(0f, 0f);
            ishurt = true;
            PlayerPrefs.SetInt("playerlife", playerlife);
            mycanvas.updatelife();
            FadeInOut.instance.SetFadeInOut("MainMenu");
        }
    }
    //碰撞到敌人，受伤或者死亡；碰撞到item
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !ishurt && canhurt)
        {
            playerlife--;
            PlayerPrefs.SetInt("playerlife", playerlife);
            mycanvas.updatelife();
            if (playerlife >= 1)
            {
                ishurt = true;
                canhurt = false;
                mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, 0.5f);
                myAnim.SetBool("Hurt", true);
                myAS.PlayOneShot(myAC[2]);
                myRigi.velocity = new Vector2(transform.localScale.x * -2.5f, 10.0f);
                StartCoroutine("Setishurt");
            }
            else if (playerlife < 1)
            { 
                myAnim.SetBool("Die",true) ;
                myAS.PlayOneShot(myAC[3]);
                myRigi.velocity = new Vector2(0f, 0f);
                ishurt=true;
                FadeInOut.instance.SetFadeInOut("MainMenu");

            }
            
            
        }
        if(collision.tag == "Item" && !ishurt && canhurt)
        {
            myAS.PlayOneShot(myAC[1]);
            
        }
        if (collision.name == "Bosscome")
        {
            Bosscome();
        }
    }
    //被敌人持续伤害
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && !ishurt && canhurt)
        {
            playerlife--;
            PlayerPrefs.SetInt("playerlife", playerlife);
            mycanvas.updatelife();
            if (playerlife >= 1)
            {
                ishurt = true;
                canhurt = false;
                mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, 0.5f);
                myAnim.SetBool("Hurt", true);
                myAS.PlayOneShot(myAC[2]);
                myRigi.velocity = new Vector2(transform.localScale.x * -2.5f, 10.0f);
                StartCoroutine("Setishurt");
            }
            else if (playerlife < 1)
            {
                myAnim.SetBool("Die", true);
                myAS.PlayOneShot(myAC[3]);
                myRigi.velocity = new Vector2(0f, 0f);
                ishurt =true;
                FadeInOut.instance.SetFadeInOut("MainMenu");

            }

        }
    }
    //受伤动画播放
    IEnumerator Setishurt()
    {
        yield return new WaitForSeconds(1.0f);
        
        ishurt =false;
        myAnim.SetBool("Hurt",false);

        yield return new WaitForSeconds(1.0f);
        canhurt = true;
        mySR.color = new Color(mySR.color.r, mySR.color.g, mySR.color.b, 1.0f);
    }
    private void Bosscome()
    {
        
        boss.SetActive(true);
    }
    //在攻击动画中调用攻击的相关逻辑
    public void Forhurtsetting()
    {
        isattack = false;
        myAnim.ResetTrigger("Attack");
        attackCollider.SetActive(false);
    }
    public void SetIsAttack()
    {
        isattack = false;
        canjump = true;
        myAnim.ResetTrigger("Attack");
    }
    public void setAttackon()
    {
        attackCollider.SetActive(true);
        myAS.PlayOneShot(myAC[0]);
    }
    public void setAttackoff()
    {
        attackCollider.SetActive(false);
    }
}
