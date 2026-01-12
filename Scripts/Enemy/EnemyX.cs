using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyX : Enemy
{
    // Start is called before the first frame update
    bool isout=false,canchase=true;
    protected override void MoveandAttack()
    {
        
        if (!isalive)
        {
            return;
        }
        if (playerscript.ishurt)
        {
            anim.SetTrigger("Idle");
            return;
        }
        if (Vector3.Distance(player.transform.position, transform.position) < 4.0f && canchase)
        {
            if (player.transform.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            else
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                
            }
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run")  )
            {
                Vector3 newtarget = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, newtarget, speed*2 * Time.deltaTime);
                if (transform.position.x < targetpoint.x && transform.position.x < Originposition.x)
                {
                    isout = true;
                }
                if (transform.position.x > targetpoint.x && transform.position.x > Originposition.x)
                {
                    isout = true;
                }
            }

            isAfterBattle = true;
            return;
        }
        else if (isAfterBattle)
        {
            if (transform.position.x < targetpoint.x)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            }
            else if (transform.position.x > targetpoint.x)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            }
            isAfterBattle = false;
           
        }
        
        if (transform.position.x == targetpoint.x )
        {
            if (!isout) 
            {
                anim.SetTrigger("Idle");
                StartCoroutine("turnbody");
                canchase=true;
            }
            
            targetpoint = Originposition;
            Originposition = transform.position;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
        {
            transform.position = Vector3.MoveTowards(transform.position, targetpoint, speed * Time.deltaTime);
            if (transform.position.x< Mathf.Max(targetpoint.x,Originposition.x) && transform.position.x >Mathf.Min(targetpoint.x, Originposition.x))
            {
                isout = false;
            }
        }

    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "PlayerAttack")
        {
            enemylife--;
            if (enemylife >= 1)
            {
                anim.SetTrigger("Hurt");
                canchase = true;
            }
            else if (enemylife < 1)
            {
                isalive = false;
                boxCollider.enabled = false;
                anim.SetTrigger("Die");
                StartCoroutine(AfterDie());
            }

        }
        if (collision.tag == "Stop")
        {
            
            canchase=false;
        }
    }
}

