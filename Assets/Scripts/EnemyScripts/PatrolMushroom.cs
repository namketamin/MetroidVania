using UnityEngine;

public class PatrolMushroom : EnemyBase
{
    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
        if (currentHp <= 0) return;
        Debug.Log(currentHp);
        if(isHurt) return;
        Flip();
        Patrol();
    }
    protected override void DirectionChange()
    {
        movingRight = !movingRight;
    }
    protected override void MoveInDirection(int dir)
    {
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir, transform.localScale.y, transform.localScale.z);
        rb.linearVelocity = new Vector2(speed * dir, rb.linearVelocity.y);
    }
    protected override void OnAfterTakeDamage(Transform attacker)
    {
        ApplyKnockback(attacker);
    }
}
