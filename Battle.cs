using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : playerControl
{
    public override void MoveAni()
    {
        base.MoveAni();

        if (Input.GetKeyDown(KeyCode.A)&& gameObject.GetComponent<BoxCollider2D>().enabled != false)
        {
            ani.SetInteger("Attack", ani.GetInteger("Attack") + 1);
        }
    }

    public override void MovementX()
    {
        base.MovementX();
        if (state.IsName("Base.Damage")) { return; }

        if (Input.GetKeyDown(KeyCode.A))
        {
            Move(0); 
            lockMove = true;
        }
    }

    public override void Move(int i)
    {
        if (CanMove())
        {
            if (Mathf.Abs(i) == 1)
            {
                playerRGB2D.velocity = new Vector2(i * speedX * Time.deltaTime, playerRGB2D.velocity.y);
            }
            else if (Mathf.Abs(i) == 2)
            {
                playerRGB2D.velocity = new Vector2(i * runSpeed * Time.deltaTime, playerRGB2D.velocity.y);
            }
            else if (Mathf.Abs(i) == 0)
            {
                playerRGB2D.velocity = new Vector2(0f, playerRGB2D.velocity.y);
            }
        }
        
        ani.SetFloat("Move", Mathf.Abs(i));
    }

    public override void Direction(int i)
    {
        if (CanMove())
        {
            transform.eulerAngles = new Vector3(0, 180f * i, 0);
        }
    }

    public bool CanMove()
    {
        return !state.IsTag("lock");
    }

    public void Damage(float dmg)
    {
        if (state.IsName("Base.damage")) {return; }
        ani.SetTrigger("Damage");
        Game.sav.Damage(dmg);
    }
}