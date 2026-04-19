using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("--Patrol--")]
    [SerializeField] protected Transform pointA;
    [SerializeField] protected Transform pointB;
    [SerializeField] protected float speed=1f;

    protected bool movingRight;

    [SerializeField] protected float idleDuration = 1f;
    protected float idleTimer;
    [Header("--Health--")]
    [SerializeField] protected int maxHp;
    public virtual int MaxHp => maxHp;
    protected int currentHp;
    [Header("--Attack--")]
    [SerializeField] protected float attackCooldown = 1f;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected float attackRange = 1f;
    [SerializeField] protected float baseDamage = 5f;
    protected float attackTimer;
    protected bool isAttacking;

    [SerializeField] protected float defenseMultiplier=1f;
    [SerializeField] protected float knockbackForce = 1f;
    [SerializeField] protected float chaseDistance = 1f;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected bool isHurt;

    protected Transform player;
    [SerializeField] protected LayerMask playerLayer;

    protected GameManager gm;
    [SerializeField] protected int scoreValue;
    protected virtual void Awake()
    {
        rb=GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gm = FindAnyObjectByType<GameManager>();
    }
    protected virtual void Start()
    {
        currentHp = maxHp;
        movingRight = true;
    }
    void Update()
    {

    }
    protected virtual void Flip()
    {
        if (rb.linearVelocityX > 0) transform.localScale = new Vector3(1f, 1f, 1f);
        else if (rb.linearVelocityX < 0) transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    protected virtual void Chasing()
    {
        anim.SetBool("isRunning", true);
        float dir = Mathf.Sign(player.transform.position.x - transform.position.x);
        rb.linearVelocity = new(dir * speed, rb.linearVelocity.y);
    }
    protected virtual void Attack() { }
    protected virtual void DealDamage() { }
    protected virtual bool PlayerInAttackRange() { return false; }
    protected virtual bool PlayerInChaseRange() { return false; }
    protected virtual void Patrol()
    {
        if (movingRight)
        {
            if (transform.position.x >= pointB.position.x)
            {
                DirectionChange();
            }
            else
            {
                MoveInDirection(1);
            }
        }
        else
        {
            if (transform.position.x <= pointA.position.x)
            {
                DirectionChange();
            }
            else
            {
                MoveInDirection(-1);
            }
        }
    }
    protected virtual void DirectionChange()
    {
        anim.SetBool("isRunning", false);
        idleTimer -= Time.deltaTime;
        if (idleTimer <= 0)
        {
            movingRight = !movingRight;
        }
    }
    protected virtual void MoveInDirection(int dir)
    {
        idleTimer = idleDuration;
        anim.SetBool("isRunning", true);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);
        rb.linearVelocity = new Vector2(speed * dir, rb.linearVelocity.y);
    }
    public virtual void TakeDamage(int dmg,Transform attacker)
    {
        anim.SetTrigger("isHurt");
        isHurt = true;

        int actualDamage = (int)(dmg * defenseMultiplier);
        currentHp-=actualDamage;

        OnAfterTakeDamage(attacker);

        if (currentHp <= 0)
        {
            Debug.Log("die");
            anim.SetTrigger("isDeath");
        }
    }
    protected virtual void OnAfterTakeDamage(Transform attacker)
    {

    }
    public virtual void ApplyKnockback(Transform attacker)
    {
        float dir = Mathf.Sign(transform.position.x - attacker.position.x);
        rb.linearVelocity = new Vector2(knockbackForce * dir, rb.linearVelocity.y);
    }
    public void EndHurt()
    {
        isHurt=false;
    }
    public void Die()
    {
        gm.AddScore(scoreValue);
        Destroy(transform.parent.gameObject);
    }
}
