using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//BOSS类
public class Boss : MonoBehaviour
{
    // Start is called before the first frame update
    
    
    float speed=2.0f;
    bool[] myState= new bool[3];
    bool[] myAnim = { true, true, true };
    public GameObject AttackCollider;
    Vector3 height,slidepoint,runpoint;
     Animator anim;
     bool Jump, isalive,Rest,isslide,isattack,canrun;
     GameObject player;
     int enemylife;
     BoxCollider2D boxCollider;
    SpriteRenderer SR;
     Player playerscript;
    AudioSource myAS;
    [SerializeField]
    AudioClip[] myAC;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        isattack = false; isalive = true;
        isslide = false;Rest = false;Jump = false;canrun = false;
        player = GameObject.Find("Player");
        
        boxCollider = GetComponent<BoxCollider2D>();
        SR = GetComponent<SpriteRenderer>();
        playerscript = player.GetComponent<Player>();
        myAS = GetComponent<AudioSource>();
        myState[0] = isattack;
        myState[1] = isslide;
        myState[2] = Jump;
        enemylife = 7;
        StartCoroutine(StateWakeControl(true,true,true));
        Lookplayer();

    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveandAttack();
    }
    //BOSS移动逻辑和攻击逻辑
     void MoveandAttack()
    {
        if (!isalive)
        {
            return;
        }
        
        if (Vector3.Distance(player.transform.position, transform.position) < 2.5f && myState[0] && myAnim[0])
        {
            //Attack
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") )
            {
                return;
            }
            Lookplayer();
            anim.SetTrigger("Attack");
            
         
            if (isattack)
            {
                StartCoroutine(StateShutControl(true, false, false));
            }
            isattack = false;
            return;
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < 5.0f && Vector3.Distance(player.transform.position, transform.position) >2.5f && myState[1] && myAnim[1])
        {
            //Slide

            if (isslide)
            {
                Lookplayer();
                slidepoint = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
                anim.SetBool("Slide", true);
                StartCoroutine(StateShutControl(false, true, false));
            }
            transform.position = Vector3.MoveTowards(transform.position, slidepoint, 3.0f * Time.deltaTime);
            isslide = false;
            return;
        }
        else if (myState[2] && myAnim[2])
        {
            //Jump
           
            transform.position = Vector3.MoveTowards(transform.position, height, 4.0f * Time.deltaTime);
            
            if (Jump)
            {
                Lookplayer();
                anim.SetBool("Run",false);
                anim.SetBool("Jump", true);
                
                height = new Vector3(player.transform.position.x, transform.position.y + 3.0f, transform.position.z);
                StartCoroutine(StateShutControl(false, false, true));
                StartCoroutine(JumpDown());
                
            }
            Jump=false;
            
            
            return;
        }
        //普通状态，Idle或者Run;
        if (myState[0] == false && myState[1] == false && myState[2] == false) 
        {
            Rest=true;
            anim.SetBool("Run",false);
        }
        else
        {
            Rest = false;
            Lookplayer();
        }
        IsCanRun();
        if (!Rest && canrun)
        {
            
            anim.SetBool("Run", true);
            runpoint= new Vector3(player.transform.position.x, transform.position.y , transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, runpoint, speed * Time.deltaTime);
        }

    }
    void Lookplayer()
    {
        //朝向问题
        if (player.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }
    void IsCanRun()
        //判断动画类型
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            canrun = true;
            myAnim[0] = true; myAnim[1] = true; myAnim[2] = true;
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            myAnim[0] = true; myAnim[1] = false; myAnim[2] = false;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {
            myAnim[1] = true; myAnim[0] = false; myAnim[2] = false;
        }
        else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jumpup") || anim.GetCurrentAnimatorStateInfo(0).IsName("JumpFall"))
        {
            myAnim[2] = true; myAnim[1] = false; myAnim[0] = false;
        }
    }
    IEnumerator StateWakeControl(bool attack,bool slide,bool jump)
    {
        //状态机，设置多个Bool值
       
        if (attack) 
        {
            yield return new WaitForSeconds(4.0f);
            myState[0] = true;
            isattack = true;
        }
        if (slide)
        {
            yield return new WaitForSeconds(5.0f);
            myState[1] = true;
            isslide = true;
        }
        if (jump)
        {
            yield return new WaitForSeconds(5.0f);
            myState[2] = true;
            Jump = true;
        }

    }
    IEnumerator StateShutControl(bool attack, bool slide, bool jump)
    {
        //状态机，设置多个Bool值
        if (attack)
        {
            yield return new WaitForSeconds(1.5f);
            myState[0] = false;
            StartCoroutine(StateWakeControl(true, false, false));
        }
        if (slide)
        {
            yield return new WaitForSeconds(1.0f);
            myState[1] = false;
            anim.SetBool("Slide", false);
            StartCoroutine(StateWakeControl(false, true, false));
        }
        if (jump)
        {
            yield return new WaitForSeconds(3.5f);
            myState[2] = false;
            anim.SetBool("Jump", false);
            StartCoroutine(StateWakeControl(false, false, true));
        }

    }
    IEnumerator JumpDown()
    {
        //
        yield return new WaitForSeconds(1.5f);
        height = new Vector3(height.x, height.y -3.0f, height.z);
        yield return new WaitForSeconds(2.0f);
        transform.position=new Vector3(transform.position.x,height.y,height.z);


    }

    IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(1.0f);
        SR.material.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(1.0f);
        SR.material.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(1.0f);
        FadeInOut.instance.SetFadeInOut("MainMenu");
        Destroy(this.gameObject);
        
        
    }
   
    public void SetAttackOn()
    {
        AttackCollider.SetActive(true);
        myAS.PlayOneShot(myAC[0]);
    }
    public void SetAttackOff()
    {
        AttackCollider.SetActive(false);
    }
    //被玩家攻击
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            enemylife--;
            if (enemylife >= 1)
            {
                anim.SetTrigger("Hurt");
            }
            else if (enemylife < 1)
            {
                myAS.PlayOneShot(myAC[1]);
                isalive = false;
                boxCollider.enabled = false;
                anim.SetTrigger("Die");
                Time.timeScale = 0.5f;
                StartCoroutine(AfterDie());
            }

        }
    }
    
}
