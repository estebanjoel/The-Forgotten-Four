using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    bool onGround;
    bool onWall;
    bool hasAttacked;
    public bool onJumpWall;
    public bool hasJumped;
    public int availableJumps;
    public bool gotDamage;
    public bool encounteredAnEnemy;
    public bool encounteredAnItemCrate;
    public bool encounteredSavingPoint;
    public bool encounteredAKey;
    public bool canMove = true;
    public bool hasFallenToACliff;
    public float speedX;
    public float speedY;
    public float myGravityScale;
    public float xForceOnJumpWall;
    float attackTriggerTime;
    public Transform leftAttackTrigger, rightAttackTrigger;
    public GameObject myAttackTrigger;
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer sprtRenderer;
    public CharStats[] partyStats;
    public bool isOnPortal;
    public AudioManager myAudioManager;
    public AudioClip[] playerAudios;
    
    // Start is called before the first frame update
    void Start()
    {
        #region Singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
        #endregion

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprtRenderer = GetComponent<SpriteRenderer>();
        for(int i = 0; i < partyStats.Length; i++)
        {
            partyStats[i] = transform.GetChild(0).transform.GetChild(i).GetComponent<CharStats>();
        }
        myAudioManager = AudioManager.instance;
        SetNewLevelValues();
    }

    private void Update() 
    {
        if(transform.parent==null)
        {
            DontDestroyOnLoad(gameObject);
        }

        if (availableJumps==1)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
                availableJumps--;
            }
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if(onJumpWall){
                rb.gravityScale=0f;
            }
            else{
                rb.gravityScale=myGravityScale;
            }
            
            Run(Input.GetAxis("Horizontal"));

            if (Input.GetButtonDown("Fire1") && !hasAttacked) Attack();

            if (hasAttacked)
            {
                if (attackTriggerTime <= 0)
                {
                    myAttackTrigger.SetActive(false);
                    hasAttacked = false;
                }

                else
                {
                    attackTriggerTime -= Time.deltaTime;
                }
            }
        }

        anim.SetFloat("speedY", rb.velocity.y);

    }

    private void LateUpdate()
    {        
        anim.SetBool("onGround", onGround);
        if (encounteredAnEnemy || gotDamage) anim.SetFloat("speedX", 0f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "MovingPlatform" || other.gameObject.tag == "Item")
        {
            onGround = true;

            if (other.gameObject.tag == "MovingPlatform")
            {
                rb.gravityScale = 0f;
                if(other.gameObject.name == "RightBound" || other.gameObject.name == "LeftBound")
                {
                    canMove = false;
                    anim.SetFloat("speedX", 0f);
                    /*if(other.gameObject.name == "RightBound")
                    {
                        rb.AddForce(transform.right * speedY, ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(transform.right * speedY * -1f, ForceMode2D.Impulse);
                    }*/
                }
                else
                {
                    transform.SetParent(other.transform);
                }
                
            }
            PlayPlayerSFX(2);
        }

        if(other.gameObject.tag == "JumpWall")
        {
            if (availableJumps==0) availableJumps++;
            onJumpWall = true;
            anim.SetBool("onJumpWall", true);
        }

        if (other.gameObject.tag == "Wall") onWall = true;

        if(other.gameObject.tag == "Enemy")
        {
            encounteredAnEnemy = true;
            canMove = false;
            anim.SetFloat("speedX", 0);
            other.gameObject.GetComponent<EnemyController>().BattleEncounter();
        }

        if(other.gameObject.tag == "Key") encounteredAKey = true;
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "Ground" || other.gameObject.tag == "MovingPlatform")
        {
            onGround = true;
            if (availableJumps==0) availableJumps++;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "MovingPlatform")
        {
            onGround = false;
            if (other.gameObject.tag == "MovingPlatform")
            {
                rb.gravityScale = myGravityScale;
                if (other.gameObject.name == "RightBound" || other.gameObject.name == "LeftBound") canMove = true;
                else this.transform.parent = null;
                
            }
            availableJumps = 0;
        }

        if (other.gameObject.tag == "JumpWall")
        {
            onJumpWall = false;
            anim.SetBool("onJumpWall", false);
        }

        if (other.gameObject.tag == "Wall") onWall = false;
        if(other.gameObject.tag == "Key") encounteredAKey = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy" && other.gameObject.layer == 9)
        {
            GetDamage();
            other.GetComponent<Bullet>().encounteredThePlayer = true;
            canMove = false;
            other.GetComponent<Bullet>().bulletFather.GetComponent<EnemyController>().SuccessOnAttack();
        }

        if (other.gameObject.name == "Portal") isOnPortal = true;

        if(other.gameObject.name == "CheckPoint") other.GetComponent<CheckPoint>().playerHasPassedThroughHere = true;

        if(other.gameObject.name == "Cliff")
        {
            for(int i = 0; i < partyStats.Length; i++)
            {
                if (partyStats[i].gameObject.activeInHierarchy)
                {
                    int currentHP = partyStats[i].GetCharStats().GetCurrentHP();
                    currentHP -= other.GetComponent<Cliff>().damageAmount;
                    if (currentHP < 1) partyStats[i].GetCharStats().SetCurrentHP(1);
                    else partyStats[i].GetCharStats().SetCurrentHP(currentHP);
                }
            }

            hasFallenToACliff = true;
            canMove = false;
        }

        if(other.gameObject.tag == "Save") encounteredSavingPoint = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Portal") isOnPortal = false;

        if(other.gameObject.tag == "Save") encounteredSavingPoint = false;
    }

    public void Run(float horizontal)
    {
        if (horizontal != 0)
        {
            if (onGround && !hasAttacked)
            {
                PlayPlayerSFX(0);
            }

            if (horizontal > 0)
            {
                sprtRenderer.flipX = false;
            }

            else if (horizontal < 0)
            {
                sprtRenderer.flipX = true;
            }
        }
        rb.velocity = new Vector2(horizontal * speedX, rb.velocity.y);
        if (!onWall)
        {
            anim.SetFloat("speedX", Mathf.Abs(horizontal));
        }
        else
        {
            anim.SetFloat("speedX", 0);
        }
    }

    public void Jump()
    {
        if(availableJumps>0)
        {
            PlayPlayerSFX(1);
            float xForce = 0;
            if(onJumpWall)
            {
                rb.gravityScale = myGravityScale;
                if (sprtRenderer.flipX)
                {
                    xForce = Vector3.right.x * xForceOnJumpWall * rb.mass;
                }
                else
                {
                    xForce = Vector3.right.x * -xForceOnJumpWall * rb.mass;
                }
            }
            float yForce = Vector3.up.y * speedY * myGravityScale * rb.mass;
            if(yForce>750){
                yForce = 750;
            }
            Vector3 jumpForce = new Vector3(xForce, yForce,0);
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
    }

    public void Attack()
    {
        PlayPlayerSFX(3);
        hasAttacked = true;
        attackTriggerTime = 0.5f;
        anim.SetTrigger("hasAttacked");
        StartCoroutine(SpawnAttackTriggerCo());
    }

    public void SpawnAttackTrigger()
    {
        if (sprtRenderer.flipX)
        {
            myAttackTrigger = rightAttackTrigger.GetChild(0).gameObject;
            myAttackTrigger.SetActive(true);
        }

        else
        {
            myAttackTrigger = leftAttackTrigger.GetChild(0).gameObject;
            myAttackTrigger.SetActive(true);
        }
    }

    public void GetDamage()
    {
        PlayPlayerSFX(4);
        anim.SetTrigger("gotDamage");
        gotDamage = true;
        canMove = false;
    }

    public IEnumerator SpawnAttackTriggerCo()
    {
        yield return new WaitForSeconds(0.25f);
        SpawnAttackTrigger();
    }

    public void SetPositionAtRespawn(Transform checkPoint)
    {
        transform.position = new Vector3(checkPoint.position.x, checkPoint.position.y, transform.position.z);
        canMove = true;
    }

    public void PlayPlayerSFX(int i)
    {
        AudioManager.instance.PlatformSFX[0].clip = playerAudios[i];
        if (!AudioManager.instance.PlatformSFX[0].isPlaying)
        {
            AudioManager.instance.PlatformSFX[0].Play();
        }
    }

    public void SetNewLevelValues()
    {
        isOnPortal = false;
        hasAttacked = false;
        canMove = true;
        hasFallenToACliff = false;
        availableJumps = 1;        
    }
}
