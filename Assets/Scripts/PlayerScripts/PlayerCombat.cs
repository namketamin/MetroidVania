using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("--Attack--")]
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] Vector2 atttackRange = new Vector2(1f, 1f);
    [SerializeField] private Transform attackPoint;
    float attackTimer;
    bool isAttacking;

    PlayerController playerCtrl;
    PlayerAudio pa;
    Rigidbody2D rb;
    Animator anim;

    [SerializeField] private LayerMask enemyLayer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerCtrl= GetComponent<PlayerController>();
        pa=GetComponentInChildren<PlayerAudio>();
    }
    void Start()
    {
        attackTimer = attackCooldown;
    }
    void Update()
    {
        DelayAttack();
    }
    void OnAttack(InputValue value)
    {
        if (value.isPressed && !playerCtrl.IsPushing)
        {
            Attack();
        }
    }
    public void Attack()
    {
        if (!isAttacking)
        {
            anim.SetTrigger("isAttacking");
            pa.PlaySFXClip(PlayerSFX.Slash);
            isAttacking = true;
            attackTimer = attackCooldown;
            //playerCtrl.jumpCount = 2;
        }
    }
    public void DealDamage()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPoint.position, atttackRange, 0f, enemyLayer);
        foreach (Collider2D hit in hits)
        {
            EnemyBase enemy = hit.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage,transform);
            }
        }
    }
    void DelayAttack()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            isAttacking = false;
        }
    }
}
