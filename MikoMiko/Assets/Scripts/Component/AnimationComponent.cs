using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : ComponentBase
{
    private Animation animation;
    private Animator animator;
    private RuntimeAnimatorController controller;
    public MikoChi miko;
    private string lastAnimator = "";
    private string lastAnimatorState = "";

    public float animationDuration = 0;

    public bool isPlaying = false;

    public float nextRandomIdelTime = 0;

    public List<AnimatorControllerParameter> animatorParameterList = new List<AnimatorControllerParameter>();

    public Dictionary<string, MikoChi.AnimationTime> animationInfoMap = new Dictionary<string, MikoChi.AnimationTime>();

    public enum AnimaState
    {
        Idle,
        Move,
        Jump,
    }

    public AnimaState currentState = AnimaState.Idle;
  //  public List<AnimatorControllerParameter> animatorParameterList = new List<AnimatorControllerParameter>();

    public override void Init(MikoChi mikochi)
    {
        miko = mikochi;
        animation = miko.animation;
        animator = miko.animator;
        animatorParameterList.Clear();
        animationInfoMap.Clear();
        
        for (int i=0; i< miko.animationTimeList.Count; ++i)
        {
            var item = miko.animationTimeList[i];
            animationInfoMap[item.animationName] = item;
        }

        var idles = MikoMikoMi.mikomikomi.idleAnimation;
        for (int i = 0; i < idles.Length; ++i)
        {
            var item = idles[i];
            animationInfoMap[item.animationName] = item;
        }

        var w = MikoMikoMi.mikomikomi.MoveAnimation;
        for (int i = 0; i < w.Length; ++i)
        {
            var item = w[i];
            animationInfoMap[item.animationName] = item;
        }

        isPlaying = false;
        nextRandomIdelTime = Time.realtimeSinceStartup +  UnityEngine.Random.Range(5, 35f);
        //throw new NotImplementedException();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAnimation(string name = "Jump")
    {
        var clip = animation.GetClip(name);
        if (clip == null) return;
        animationDuration = clip.length;
       /// Debug.Log(animator.GetInteger("moving"));
        animationDuration = animation.clip.length;
        animation.Play(name);

    }

    public void PlayAnimator(string name = "Jump", bool loop = false)
    {
        if (currentState != AnimaState.Idle )
            return;
        if (!animationInfoMap.ContainsKey(name)) return;
        isPlaying = true;
        animator.CrossFade(name, 0.1f);
        var s = animator.GetCurrentAnimatorStateInfo(0);
        animationDuration = animationInfoMap[name].animationTime;
        lastAnimator = name;

        if (name == "flex")
            FaceManager.instance.ChangeFace(animationInfoMap[name].faceName);
    }

    public void ChangeAnimatorState(string name)
    {
        if (lastAnimatorState == name)
            return;
        if (lastAnimatorState == null)
            return;
        animator.SetInteger(lastAnimatorState, 0);
        animator.SetInteger(name, 1);
        if (name == "jump")
        {
            animator.Play("jump2");

        }
        if (name == "walk" /*&& lastAnimatorState != "jump"*/)
        {
            animator.CrossFade("walk", 0.15f);
            FaceManager.instance.ChangeFace("Normal");
        }

        lastAnimatorState = name;
    }

    public void ChangeState(AnimaState state)
    {
        if (currentState != AnimaState.Idle && state == AnimaState.Idle)
        {
            if (nextRandomIdelTime - Time.realtimeSinceStartup < 2f)
                nextRandomIdelTime += 2;
            if (currentState == AnimaState.Move)
            {
                animator.Play("Idle");
                FaceManager.instance.ChangeFace("Normal");
            }

        }
        currentState = state;
    }


    public override void Update(float deltatime)
    {
        float curtime = Time.realtimeSinceStartup;
        if (isPlaying && animationDuration > 0)
        {
            animationDuration -= deltatime;
            if (animationDuration < 0)
            {
                animator.CrossFade("Idle", 0.125f);
                FaceManager.instance.ChangeFace("Normal");

                lastAnimator = "Idle";
                isPlaying = false;
                if (nextRandomIdelTime - curtime < 2f)
                    nextRandomIdelTime += 2; 
            }
        }

        if (!isPlaying && lastAnimator == "Idle" && curtime > nextRandomIdelTime && currentState == AnimaState.Idle)
        {
            nextRandomIdelTime = curtime + UnityEngine.Random.Range(5, 35f);
            var idles = MikoMikoMi.mikomikomi.idleAnimation;
            int i = UnityEngine.Random.Range(0, idles.Length);

            PlayAnimator(idles[i].animationName);
        }
    }

    public override void Destory()
    {
        throw new NotImplementedException();
    }
}
