using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallSliderState : PlayerState
{
    public PlayerWallSliderState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.velocity = new Vector2(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsWallDetected() == false)//修复当墙不存在也没有落地时，依旧做滑墙动作的bug
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {

            stateMachine.ChangeState(player.wallJump);
            return;
        }

        if(xInput != 0&& player.facingDir != xInput)
        {
          
              stateMachine.ChangeState(player.idolState);
           
        }

        if (yInput < 0)
        {
            rb.velocity=new Vector2(0,rb.velocity.y);
        }
        else
        rb.velocity=new Vector2(0,rb.velocity.y*.7f);

        if (player.IsGroundDetected()) {

            stateMachine.ChangeState(player.idolState);
        }
    }
}
