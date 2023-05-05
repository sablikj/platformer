using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public float moveSpeed;
    public Rigidbody2D RB;
    public float jumpForce;

    private bool isGrounded;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;

    private bool canDoubleJump;
    private SpriteRenderer SR;

    public float knockBackLength, knockBackForce;
    private float knockBackCounter;

    private Animator anim;

    public float bounceForce;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (knockBackCounter <= 0)
        {
            RB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), RB.velocity.y);

            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);

            if (isGrounded)
            {
                canDoubleJump = true;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    RB.velocity = new Vector2(RB.velocity.x, jumpForce);
                    AudioManager.instance.PlaySFX(10);
                }
                else
                {
                    if (canDoubleJump)
                    {
                        RB.velocity = new Vector2(RB.velocity.x, jumpForce);
                        canDoubleJump = false;
                        AudioManager.instance.PlaySFX(10);
                    }
                }
            }

            if (RB.velocity.x < 0)
            {
                SR.flipX = true;
            }
            else if (RB.velocity.x > 0)
            {
                SR.flipX = false;
            }
        }
        else
        {
            knockBackCounter -= Time.deltaTime;
            if(!SR.flipX) 
            {
                RB.velocity = new Vector2(-knockBackForce, RB.velocity.y);
            }
            else
            {
                RB.velocity = new Vector2(knockBackForce, RB.velocity.y);
            }
        }
        anim.SetFloat("moveSpeed", Mathf.Abs(RB.velocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void KnockBack()
    {
        knockBackCounter = knockBackLength;
        RB.velocity = new Vector2(0f, knockBackForce);

        anim.SetTrigger("hurt");
    }

    public void Bounce()
    {
        RB.velocity = new Vector2(RB.velocity.x, bounceForce);
        AudioManager.instance.PlaySFX(10);
    }
}
