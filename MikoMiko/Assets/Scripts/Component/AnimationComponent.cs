using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : ComponentBase
{
    private Animation animation;
    private Animator animator;
    public MikoChi miko;
    private string lastAnimator = "";

    public float animationDuration = 0;
    
    public override void Init(MikoChi mikochi)
    {
        miko = mikochi;
        animation = miko.animation;
        animator = miko.animator;

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

    public void PlayAnimator(string name = "Jump")
    {
        animator.SetInteger(lastAnimator, 0);
        animator.SetInteger(name, 1);
        lastAnimator = name;
    }


    public override void Update(float deltatime)
    {
        if (animation.isPlaying && animationDuration > 0)
        {
            animationDuration -= deltatime;
            if (animationDuration < 0)
                animation.Stop();
        }
    }

    public override void Destory()
    {
        throw new NotImplementedException();
    }
}
