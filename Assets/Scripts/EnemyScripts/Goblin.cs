using System.Collections;
using UnityEngine;

public class Goblin : EnemyBase
{
    [SerializeField] private float colliderDistance=1f;
    [SerializeField] private float range = 1f;
    [SerializeField] private BoxCollider2D boxCol;
    protected override void Awake()
    {
        base.Awake();
        defenseMultiplier = 1f;
    }
    protected override void Start()
    {
        base.Start();
        attackTimer = attackCooldown;
    }
    void FixedUpdate()
    {

    }
    void Update()
    {
        if (isHurt) return;
        attackTimer-= Time.deltaTime;
        if (PlayerInAttackRange())
        {
            LookAtPlayer();
            Attack();
            return;
        }
        else if (PlayerInChaseRange())
        {
            Chasing();
            Flip();
            return;
        }
        Patrol();
    }
    protected private void LookAtPlayer()
    {
        float dir = player.transform.position.x - transform.position.x;
        if (dir > 0f && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (dir < 0f && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    protected override void Attack()
    {
        anim.SetBool("isRunning", false);
        if (attackTimer <= 0)
        {
            anim.SetTrigger("isAttacking");
            attackTimer = attackCooldown;
        }
    }
    protected override void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        foreach (Collider2D hit in hits)
        {
            PlayerHealth ph = hit.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(baseDamage, transform);
            }
        }
    }
    protected override bool PlayerInAttackRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCol.bounds.center + transform.right * colliderDistance * transform.localScale.x,
            new Vector3(boxCol.bounds.size.x * range, boxCol.bounds.size.y, boxCol.bounds.size.z), 0, Vector2.right, 0, playerLayer);
        if (hit.collider != null)
        {
            return true;
        }
        return false;
    }
    protected override bool PlayerInChaseRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position,new Vector2(transform.localScale.x,0),chaseDistance,playerLayer);
        if (hit.collider!=null)
        {
            return true;
        }
        return false;
    }
    protected override void OnAfterTakeDamage(Transform attacker)
    {
        ApplyKnockback(attacker);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(boxCol.bounds.center + transform.right * colliderDistance * transform.localScale.x,
            new Vector3(boxCol.bounds.size.x * range, boxCol.bounds.size.y, boxCol.bounds.size.z));

        Vector3 dir = new Vector3(transform.localScale.x, 0f, 0f);
        Gizmos.DrawLine(transform.position,transform.position+dir*chaseDistance);

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
