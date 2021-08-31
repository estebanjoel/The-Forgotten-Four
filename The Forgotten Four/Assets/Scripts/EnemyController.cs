using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeed;
    public float leftBound, rightBound;
    private float moveTarget;
    public float flipTimer, fireRate;
    private float remainingTimeToFlip, remainingTimeToFire;
    public bool hasFlipped, canAttack;
    public bool isBoss;
    public bool canMove = true;
    public bool gotDamage = false;
    public bool attackedSuccesfully;
    public bool encounteredThePlayer;
    public Bullet bullet;
    public string[] enemyParty;
    public Animator anim;
    public AudioClip hurtAudio;
    public AudioClip bulletAudio;
    
    void Start()
    {
        moveTarget = leftBound;
        anim = GetComponent<Animator>();
        remainingTimeToFlip = flipTimer;
        remainingTimeToFire = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            MoveThroughTarget();
        }

        if (!encounteredThePlayer && !gotDamage)
        {
            CheckAttackTarget();
        }
    }

    public void MoveThroughTarget()
    {        
        if (moveTarget == leftBound)
        {
            if (transform.position.x <= moveTarget)
            {
                if (!hasFlipped)
                {
                    if (anim != null)
                    {
                        anim.SetTrigger("flip");
                    }
                    hasFlipped = true;
                }
                else { 
                    
                    if (remainingTimeToFlip <= 0)
                    {
                        moveTarget = rightBound;
                        remainingTimeToFlip = flipTimer;
                        GetComponent<SpriteRenderer>().flipX = true;
                        hasFlipped = false;
                    }
                    else
                    {
                        remainingTimeToFlip -= Time.deltaTime;
                    }
                }
            }
            else
            {
                transform.position -= transform.right * moveSpeed * Time.deltaTime;
            }
        }

        if (moveTarget == rightBound)
        {
            if (transform.position.x >= moveTarget)
            {
                if (!hasFlipped)
                {
                    if (anim != null)
                    {
                        anim.SetTrigger("flip");
                    }
                    hasFlipped = true;
                }
                else
                {

                    if (remainingTimeToFlip <= 0)
                    {
                        moveTarget = leftBound;
                        remainingTimeToFlip = flipTimer;
                        GetComponent<SpriteRenderer>().flipX = false;
                        hasFlipped = false;
                    }
                    else
                    {
                        remainingTimeToFlip -= Time.deltaTime;
                    }
                }
            }
            else
            {
                transform.position += transform.right * moveSpeed * Time.deltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            canMove = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            AudioManager.instance.PlatformSFX[1].clip = hurtAudio;
            AudioManager.instance.PlatformSFX[1].Play();
            if(anim!=null) anim.SetTrigger("damage");
            gotDamage = true;
            canMove = false;
        }
    }

    public void CheckAttackTarget()
    {
        if(PlayerController.instance!=null)
        {
            Vector3 target = PlayerController.instance.transform.position;
            Vector3 positionInWorld = transform.TransformDirection(transform.position.x, transform.position.y, transform.position.z);
            float xDistance = target.x - positionInWorld.x;
            float yDistance = target.y - positionInWorld.y;
            if (yDistance <=2 && yDistance>= -2)
            {
                if (xDistance <= 10 && xDistance >= -10)
                {
                    if (xDistance < 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = false;
                        if (!attackedSuccesfully)
                        {
                            if (remainingTimeToFire <= 0)
                            {
                                Attack(-1f);
                                remainingTimeToFire = fireRate;
                            }
                            else
                            {
                                remainingTimeToFire -= Time.deltaTime;
                            }
                        }
                    }

                    else if (xDistance > 0)
                    {
                        GetComponent<SpriteRenderer>().flipX = true;
                        if (!attackedSuccesfully)
                        {
                            if (remainingTimeToFire <= 0)
                            {
                                if (canAttack)
                                {
                                    Attack(1f);
                                }
                                remainingTimeToFire = fireRate;
                            }
                            else
                            {
                                remainingTimeToFire -= Time.deltaTime;
                            }
                        }
                    }
                canMove = false;
                }
            
            }
            else
            {
                canMove = true;
            }
        }
    }

    public void Attack(float direction)
    {
        AudioManager.instance.PlatformSFX[1].clip = bulletAudio;
        AudioManager.instance.PlatformSFX[1].Play();
        Bullet enemyBullet = Instantiate(bullet);
        enemyBullet.xDirection = direction;
        enemyBullet.movesOnX = true;
        enemyBullet.gameObject.tag = "Enemy";
        enemyBullet.transform.position = transform.position;
        enemyBullet.bulletFather = transform;
    }

    public void BattleEncounter()
    {
        encounteredThePlayer = true;
    }

    public void SuccessOnAttack()
    {
        attackedSuccesfully = true;
    }
}
