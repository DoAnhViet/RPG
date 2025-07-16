using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public int facingDirection = 1;

    public Player_Combat player_Combat;

    private void Update()
    {
        if (Input.GetButtonDown("Slash"))
        {
            player_Combat.Attack();
        }
    }
    // Fixed Update is called 50 frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if(horizontal > 0 && transform.localScale.x < 0 ||
           horizontal < 0 && transform.localScale.x > 0)
        {
            Flip();
        }


        anim.SetFloat("horizontal", Mathf.Abs(horizontal));
        anim.SetFloat("vertical", Mathf.Abs(vertical));


        rb.linearVelocity = new Vector2(horizontal, vertical) * StatsManager.Instance.speed;
    }
    void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
