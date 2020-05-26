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

       
        if (Input.GetKeyUp(KeyCode.Space) && !isJumping)
        {
            DoJump();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
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
            if (isJumping)
        {
            startPosition.y -=  velocity.y;
            WindowSetting.instance.DoMoveWindow(startPosition.x, startPosition.y);
            ++flag;
            velocity.y--;
            //if (flag%10 == 0)
            //{
            //    velocity.y--;
            //    if (velocity.y == 0)
            //        velocity.y = -1;
            //}

            if (velocity.y == -8)
            {
                isJumping = false;
            }
        }
    }


    public void DoAction()
    {



        if (currentAction == MikoAction.MoveLeft || currentAction == MikoAction.MoveRight)
        {
            var r = GameEngine.instance.miko.body.localEulerAngles;
            r.y = 0;
            r.z = currentAction == MikoAction.MoveLeft ? 30f : -30f;
            GameEngine.instance.miko.SetRotation(r);
            GameEngine.instance.miko.PlayAnimator("walk");
        }

        if (currentAction == MikoAction.Idle)
        {
            var r = GameEngine.instance.miko.body.localEulerAngles;
            r.z = r.y = 0;
            GameEngine.instance.miko.SetRotation(r);
            GameEngine.instance.miko.PlayAnimator("Idle");

        }
    }

    public void DoJump()
    {
        var rect = WindowSetting.instance.GetCurrentWindowPos();
        startPosition.x = rect.Left;
        startPosition.y = rect.Top;
        flag = 0;
        velocity.y = 15;
        isJumping = true;
        GameEngine.instance.miko.PlayAnimation();
    }

    public void RandomMove()
    {

    }
}
