using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikoChi : MonoBehaviour
{
    public AudioSource audio;
    // Start is called before the first frame update
    public Material mikoFace = null;

    public SkinnedMeshRenderer mikoRenderer = null;

    public Texture[] mikoFaceArray;

    public int currentFace = 0;
    private bool canPlayVoice = true;
    public AudioClip[] audioClips;
    public Animation animation;
    public Animator animator;
    public List<string> animatorParameters;
    private bool OnAnimator = false;

    public Transform body;
    public enum FaceType{
        Cry = 1,
        Angry =3,
    }

    public List<int> audioToFace;
    void Start()
    {
        canPlayVoice = true;
                mikoRenderer.material = mikoFace;
        mikoFace.SetTexture("__Tex", mikoFaceArray[5]);

        ChangeAnimatorState("nyahello", true);
        //PlayAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  public void ChangeTexture()
    {
        if (currentFace >= mikoFaceArray.Length)
            currentFace = 0;
        mikoFace.mainTexture = mikoFaceArray[currentFace++];
    }

    public void PlayVoice(int index = 0){
        if (!canPlayVoice) return;
      //  canPlayVoice = false;
        if (index == 0)
            index = Random.Range(0, 2);
        audio.clip = audioClips[index];
        Debug.Log(audioClips[index].length);
        TimerManager.instance.AddTimer(audioClips[index].length, -1, FaceChangeCallBack);

        audio.Play();


    }

    public void PlayVoiceByName(string str = ""){
        audio.Play();
        
    }

    public void FaceChange(int faceId){
        Debug.Log(faceId);
        if (faceId >= mikoFaceArray.Length)
            currentFace = 0;
        currentFace = faceId;
        mikoFace.SetTexture("__Tex", mikoFaceArray[faceId]);
    }
    public void FaceChangeCallBack(int faceId){
        canPlayVoice = true;
        if (faceId == -1)
            return;
        FaceChange(faceId);
    }

    public void PlayAnimation(int index = 0){
        animation.Play("ShakeChestWithHead2");
        //FaceChange(8);
    }


    public void ChangeAnimatorState(string key, bool value){
        if (OnAnimator)
            return;
        OnAnimator = true;
        animator.SetBool(key, value);
        animator.Play(key);
    }

    public void ChangeFaqState(){

      //  animator.Play(key);
        animator.SetBool("Faq", false);
        animator.Play("Idel");

    }

    public void SetAnimationBool(int id, bool b){
        animator.SetBool(animatorParameters[id], b);
    }

    public void SetAnimationBoolToFalse(int id){
        OnAnimator = false;

        animator.SetBool(animatorParameters[id], false);
        animator.Play("Idel");


    }

    public void SetAnimationBoolToTrue(int id){
        animator.SetBool(animatorParameters[id], true);
      //  animator.Play("Idel");

    }

    
}
