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

    public float animationDuration = 0;

    public bool isPlaying = false;

    public List<AnimatorControllerParameter> animatorParameterList = new List<AnimatorControllerParameter>();

    public Dictionary<string, float> animationDurationMap = new Dictionary<string, float>();
  //  public List<AnimatorControllerParameter> animatorParameterList = new List<AnimatorControllerParameter>();

    public override void Init(MikoChi mikochi)
    {
        miko = mikochi;
        animation = miko.animation;
        animator = miko.animator;
        animatorParameterList.Clear();
        animationDurationMap.Clear();
        
        for (int i=0; i< miko.animationTimeList.Count; ++i)
        {
            var item = miko.animationTimeList[i];
            animationDurationMap[item.animationName] = item.animationTime;
        }
        isPlaying = false;
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
        if (!animationDurationMap.ContainsKey(name)) return;
        isPlaying = true;
        animator.CrossFade(name, 0.1f);
        var s = animator.GetCurrentAnimatorStateInfo(0);
        animationDuration = animationDurationMap[name];
        lastAnimator = name;
    }


    public override void Update(float deltatime)
    {
        if (isPlaying && animationDuration > 0)
        {
            animationDuration -= deltatime;
            if (animationDuration < 0)
            {
                animator.CrossFade("Idle", 0.1f);
                isPlaying = false;
            }
        }
    }

    public override void Destory()
    {
        throw new NotImplementedException();
    }
}
