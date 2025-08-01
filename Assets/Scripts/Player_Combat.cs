using UnityEngine;
using static Unity.Cinemachine.IInputAxisOwner.AxisDescriptor;

public class Player_Combat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayer;
    public StatsUI statsUI;
    public Animator anim;
    public float knockbackForce = 50;
    public float cooldown = 1;
    private float timer;
    public float stunTime = 1;
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }
    public void Attack() 
    {
        if (timer <= 0)
        {
            anim.SetBool("isAttacking", true);

            timer = cooldown;
        }
    }
    public void DealDamage()
    {
        //StatsManager.Instance.damage += 1;
        statsUI.UpdateDamage(); 
            Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, StatsManager.Instance.weaponRange, enemyLayer);
            if (enemies.Length > 0)
            {
                enemies[0].GetComponent<Enemy_Health>().ChangeHealth(-StatsManager.Instance.damage);
             enemies[0].GetComponent<Enemy_Knockback>().Knockback(transform, knockbackForce, stunTime);
        }

    }
    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, StatsManager.Instance.weaponRange);
    }
}
