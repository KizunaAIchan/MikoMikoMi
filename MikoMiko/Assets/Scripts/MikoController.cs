using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikoController : MonoBehaviour
{

    private float actionDuration = 0;
    public Vector2Int velocity = new Vector2Int(0, 5);
    public Vector2 acceleration = Vector2.zero;
    public bool isJumping = false;

    public Vector2Int startPosition = new Vector2Int();
    private int flag = 0;

    public enum MikoAction
    {
        Idle,
        MoveLeft,
        MoveRight,
        Jump,
        JumpLeft,
        JumpRight,
    }


    public MikoAction currentAction = MikoAction.Idle;

    [SerializeField]
    public int speed =5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isJumping && actionDuration > 0)
        {
            actionDuration -= Time.deltaTime;
            if (actionDuration < 0)
                isJumping = false;
        }
       
        if (Input.GetKeyUp(KeyCode.Space) && !isJumping)
        {
            DoJump();
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            var rect = WindowSetting.instance.GetCurrentWindowPos();
            WindowSetting.instance.DoMoveWindow(rect.Left + speed, rect.Top);
            currentAction = MikoAction.MoveRight;

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentAction = MikoAction.MoveLeft;

            var rect = WindowSetting.instance.GetCurrentWindowPos();
            WindowSetting.instance.DoMoveWindow(rect.Left - speed, rect.Top);
        }
        else
        {
            currentAction = MikoAction.Idle;
        }

        DoAction();
    }


    public void DoAction()
    {



        if (currentAction == MikoAction.MoveLeft || currentAction == MikoAction.MoveRight)
        {
            var r = GameEngine.instance.miko.body.localEulerAngles;
          //  r.y = 0;
            r.y = currentAction == MikoAction.MoveLeft ? 30f : -30f;
            GameEngine.instance.miko.SetRotation(r);
            if (!isJumping)
            {
                GameEngine.instance.miko.animationComponent.ChangeState(AnimationComponent.AnimaState.Move);
                GameEngine.instance.miko.animationComponent.ChangeAnimatorState("walk");
            }

        }

        if (currentAction == MikoAction.Idle)
        {
            var r = GameEngine.instance.miko.body.localEulerAngles;
            r.z = r.y = 0;
            GameEngine.instance.miko.SetRotation(r);

            if (!isJumping)
            {
                GameEngine.instance.miko.animationComponent.ChangeAnimatorState("Idle");

                GameEngine.instance.miko.animationComponent.ChangeState(AnimationComponent.AnimaState.Idle);
            }
  
        }
    }

    public void DoJump()
    {
        //var rect = WindowSetting.instance.GetCurrentWindowPos();
        //startPosition.x = rect.Left;
        //startPosition.y = rect.Top;
        //flag = 0;
        //velocity.y = 15;
        isJumping = true;
        actionDuration = 0.8f;
        GameEngine.instance.miko.animationComponent.ChangeState(AnimationComponent.AnimaState.Jump);
        GameEngine.instance.miko.animationComponent.ChangeAnimatorState("jump");
    }

    public void RandomMove()
    {

    }
}
