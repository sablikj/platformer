using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTankController : MonoBehaviour
{
    public enum states { shooting, hurt, moving, ended };
    public states currentState;

    public Transform boss;
    public Animator anim;

    [Header("Movement")]
    public float moveSpeed;
    public Transform leftPoint, rightPoint;
    private bool moveRight;

    [Header("Mine")]
    public GameObject mine;
    public Transform minePoint;
    public float timeBetweenMines;
    private float mineCounter;

    [Header("Shooting")]
    public GameObject bullet;
    public Transform firePoint;
    public float timeBetweenShots;
    private float shotCounter;

    [Header("Hurt")]
    public float hurtTime;
    private float hurtCounter;
    public GameObject hitBox;

    [Header("Health")]
    public int health = 5;
    public GameObject explosion, winPlatform;
    private bool isDefeated;
    public float shotSpeedUp, mineSpeedUp;


    // Start is called before the first frame update
    void Start()
    {
        currentState = states.shooting;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case states.shooting:
                shotCounter -= Time.deltaTime;
                if(shotCounter <= 0 )
                {
                    shotCounter = timeBetweenShots;
                    var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);

                    // Setting correct scale (facing correct side)
                    newBullet.transform.localScale = boss.localScale;
                }

                break;

            case states.hurt: 
                if(hurtCounter > 0)
                {
                    hurtCounter -= Time.deltaTime;

                    if(hurtCounter <= 0)
                    {
                        currentState = states.moving;
                        mineCounter = 0;

                        if(isDefeated)
                        {
                            boss.gameObject.SetActive(false);
                            Instantiate(explosion, boss.position, boss.rotation);                          
                            winPlatform.SetActive(true);
                            AudioManager.instance.StopBossMusic();

                            currentState = states.ended;
                        }
                    }
                }
                break;

            case states.moving:
                if(moveRight) 
                {
                    boss.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

                    if(boss.position.x > rightPoint.position.x)
                    {
                        // Flipping
                        boss.localScale = new Vector3(1f, 1f, 1f);

                        moveRight = false;
                        EndMovement();
                    }
                }
                else
                {
                    boss.position -= new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

                    if (boss.position.x < leftPoint.position.x)
                    {
                        // Flipping
                        boss.localScale = new Vector3(-1f, 1f, 1f);

                        moveRight = true;
                        EndMovement();
                    }
                }
                // Dropping mines when moving
                mineCounter -= Time.deltaTime;
                if(mineCounter <= 0)
                {
                    mineCounter = timeBetweenMines;
                    Instantiate(mine, minePoint.position, minePoint.rotation);
                }

                break;
        }
    }

    public void TakeHit()
    {
        currentState = states.hurt;
        hurtCounter = hurtTime;

        anim.SetTrigger("Hit");
        AudioManager.instance.PlaySFX(0);

        BossTankMine[] mines = FindObjectsOfType<BossTankMine>();
        if(mines.Length > 0)
        {
            foreach (BossTankMine mine in mines)
            {
                mine.Explode();
            }
        }

        health--;
        if(health <= 0)
        {
            isDefeated = true;
        }
        else
        {
            // Increasing difficulty after each hit
            timeBetweenShots /= shotSpeedUp;
            timeBetweenMines /= mineSpeedUp;
        }
    }

    private void EndMovement()
    {
        currentState = states.shooting;
        // Starts shooting right after flip
        shotCounter = 0f;
        anim.SetTrigger("StopMoving");
        hitBox.SetActive(true);
    }
}
