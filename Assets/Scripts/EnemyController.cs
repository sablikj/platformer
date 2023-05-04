using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public Transform leftPoint, rightPoint;
    private bool movingRight;

    private Rigidbody2D RB;
    public SpriteRenderer SR;
    private Animator anim;

    public float moveTime, waitTime;
    private float moveCount, waitCount;

    // Start is called before the first frame update
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // Points no longer children of enemy - not changing
        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight= true;

        moveCount = moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCount > 0)
        {
            moveCount -= Time.deltaTime;

            if (movingRight)
            {
                RB.velocity = new Vector2(moveSpeed, RB.velocity.y);
                SR.flipX = true;

                if (transform.position.x > rightPoint.position.x)
                {
                    movingRight = false;
                }
            }
            else
            {
                RB.velocity = new Vector2(-moveSpeed, RB.velocity.y);
                SR.flipX = false;

                if (transform.position.x < leftPoint.position.x)
                {
                    movingRight = true;
                }
            }

            if(moveCount <= 0)
            {
                waitCount = Random.Range(waitTime * .75f, waitTime * 1.25f);
            }

            anim.SetBool("isMoving", true);
        }
        else if(waitCount > 0)
        {
            waitCount -= Time.deltaTime;
            RB.velocity = new Vector2(0f, RB.velocity.y);

            if(waitCount <= 0)
            {
                moveCount = Random.Range(moveTime * .75f, waitTime * 1.25f); ;
            }
            anim.SetBool("isMoving", false);
        }
    }
}
