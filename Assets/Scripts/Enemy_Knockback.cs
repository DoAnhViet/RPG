using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private EnemyMovement enemy_Movement;
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy_Movement = GetComponent<EnemyMovement>();
    }
    public void Knockback(Transform playerTransfrom, float knockbackForce, float stunTime)
    {
        enemy_Movement.ChangeState(EnemyMovement.EnemyState.Knockback);
        StartCoroutine(StunTimer(stunTime));
        Vector2 direction = (transform.position - playerTransfrom.position).normalized;
        rb.linearVelocity = direction * knockbackForce;

    }
    IEnumerator StunTimer(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        enemy_Movement.ChangeState(EnemyMovement.EnemyState.Idle);
    }
}
