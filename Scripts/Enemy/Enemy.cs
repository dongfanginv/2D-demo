using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//敌人的基类
public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
   
    public Vector3 targetpoint;
    [HideInInspector]
    public float speed;
    public GameObject  AttackCollider;
    protected Vector3 Originposition;
    protected Animator anim;
   protected bool  isAfterBattle,isalive;
    protected GameObject player;
    protected int enemylife;
    protected BoxCollider2D boxCollider;
    protected SpriteRenderer SR;
    protected Player playerscript;
    protected AudioSource myAS;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        Originposition = transform.position;
        isAfterBattle = false; isalive=true;
        player = GameObject.Find("Player");
        StartCoroutine("turnbody");
        boxCollider = GetComponent<BoxCollider2D>();
        SR=GetComponent<SpriteRenderer>();
        playerscript = player.GetComponent<Player>();
        myAS=GetComponent<AudioSource>();
        enemylife = 3;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveandAttack();
    }
    //执行自动巡逻以及发现玩家进行攻击的逻辑
    protected virtual void MoveandAttack()
    {
        if (!isalive)
        {
            return;
        }
        if (Vector3.Distance(player.transform.position, transform.position) < 2.3f)
        {

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("AttackWait"))
            {
                return;
            }
            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            anim.SetTrigger("Attack");
            isAfterBattle = true;
            return;
        }
        else if (isAfterBattle)
        {
            if (transform.position.x < targetpoint.x)
            {

                StartCoroutine(turnbody2(true));
            }
            else if (transform.position.x > targetpoint.x)
            {
                StartCoroutine(turnbody2(false));
            }
            isAfterBattle = false;
        }

        if (transform.position.x == targetpoint.x)
        {

            anim.SetTrigger("Idle");
            targetpoint = Originposition;
            Originposition = transform.position;

            StartCoroutine(turnbody());

        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetpoint, speed * Time.deltaTime);

        }
    }
    //执行转身动画
    protected IEnumerator turnbody()
    {
        yield return new WaitForSeconds(1.5f);
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, 1.0f, 1.0f);
    }
    protected IEnumerator turnbody2(bool turnright)
    {
        if (turnright)
        {
            yield return new WaitForSeconds(1.5f);
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            transform.localScale = new Vector3( -1.0f, 1.0f, 1.0f);
        }
        
    }
    //敌人死亡后
    protected IEnumerator AfterDie()
    {
        yield return new WaitForSeconds(1.0f);
        SR.material.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(1.0f);
        SR.material.color = new Color(1, 1, 1, 0.2f);
        yield return new WaitForSeconds(1.0f);
        Destroy(this.gameObject);
    }
    public void SetAttackOn()
    {
        AttackCollider.SetActive(true);
        myAS.PlayOneShot(myAS.clip);
    }
    public void SetAttackOff()
    {
        AttackCollider.SetActive(false);
    }
    //被主角攻击减少生命
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            enemylife--;
            if(enemylife>= 1)
            {
                anim.SetTrigger("Hurt");
            }
            else if(enemylife < 1)
            {
                isalive = false;
                boxCollider.enabled = false;
                anim.SetTrigger("Die");
                StartCoroutine(AfterDie());
            }
            
        }
    }
}
