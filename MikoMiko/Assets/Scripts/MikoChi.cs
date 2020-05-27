using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MikoChi : MonoBehaviour
{
  //  public AudioSource audio;
    // Start is called before the first frame update
    public Material mikoFace = null;

    public SkinnedMeshRenderer mikoRenderer = null;

    public Texture[] mikoFaceArray;

    public int currentFace = 0;
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


    public AudioComponent audio;
    public AnimationComponent animationComponent;

    public List<ComponentBase> componentList = new List<ComponentBase>();
    public int love = 0;

    void Start()
    {
        //canPlayVoice = true;
        //        mikoRenderer.material = mikoFace;
        //mikoFace.SetTexture("__Tex", mikoFaceArray[5]);

        //ChangeAnimatorState("nyahello", true);
        //PlayAnimation();
    }

    public void InitMikoChi()
    {
        audio = new AudioComponent();
        animationComponent = new AnimationComponent();
        componentList.Add(audio);
        componentList.Add(animationComponent);


        for(int i =0; i< componentList.Count; ++i)
        {
            componentList[i].Init(this);
        }



        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Hajimaruyo, EventReceiver, 1);
        EventManager.instance.AddListener((int)EventManager.EventSender.MikoChi, (int)EventManager.EventType.MikoChi_Oyasumi, EventReceiver, 1);
    }


    void Update()
    {
        float dlt = Time.deltaTime;
        for (int i = 0; i < componentList.Count; ++i)
        {
            componentList[i].Update(dlt);
        }
    }






    public void ChangeTexture()
    {
        if (currentFace >= mikoFaceArray.Length)
            currentFace = 0;
        mikoFace.mainTexture = mikoFaceArray[currentFace++];
    }

    //public void PlayVoice(int index = 0){
    //    if (!canPlayVoice) return;
    //  //  canPlayVoice = false;
    //    if (index == 0)
    //        index = Random.Range(0, 2);
    //    audio.clip = audioClips[index];
    //    Debug.Log(audioClips[index].length);
    //    TimerManager.instance.AddTimer(audioClips[index].length, -1, FaceChangeCallBack);

    //    audio.Play();


    //}

    //public void PlayVoiceByName(string str = ""){
    //    audio.Play();

    //}

    //public void FaceChange(int faceId){
    //    Debug.Log(faceId);
    //    if (faceId >= mikoFaceArray.Length)
    //        currentFace = 0;
    //    currentFace = faceId;
    //    mikoFace.SetTexture("__Tex", mikoFaceArray[faceId]);
    //}
    //public void FaceChangeCallBack(int faceId){
    //    canPlayVoice = true;
    //    if (faceId == -1)
    //        return;
    //    FaceChange(faceId);
    //}

    public void PlayAnimation(string name = "Jump")
    {
        animationComponent.PlayAnimation(name);
        //FaceChange(8);
    }

    public void PlayAnimator(string name = "Jump")
    {
        animationComponent.PlayAnimator(name);
        //FaceChange(8);
    }
    //public void ChangeAnimatorState(string key, bool value){
    //    if (OnAnimator)
    //        return;
    //    OnAnimator = true;
    //    animator.SetBool(key, value);
    //    animator.Play(key);
    //}

    //public void ChangeFaqState(){

    //  //  animator.Play(key);
    //    animator.SetBool("Faq", false);
    //    animator.Play("Idel");

    //}

    //public void SetAnimationBool(int id, bool b){
    //    animator.SetBool(animatorParameters[id], b);
    //}

    //public void SetAnimationBoolToFalse(int id){
    //    OnAnimator = false;

    //    animator.SetBool(animatorParameters[id], false);
    //    animator.Play("Idel");


    //}

    //public void SetAnimationBoolToTrue(int id){
    //    animator.SetBool(animatorParameters[id], true);
    //  //  animator.Play("Idel");

    //}

    public void EventReceiver(int id, object args)
    {
        switch (id)
        {
            case (int)EventManager.EventType.MikoChi_Hajimaruyo:
                {
                    ChannelConfig cf = (ChannelConfig)args;
                    PlayAudio(cf.startNotification);
                    break;
                }
            case (int)EventManager.EventType.MikoChi_Oyasumi:
                {
                    ChannelConfig cf = (ChannelConfig)args;

                    PlayAudio(cf.closureNotice);

                    break;
                }
            default:
                break;
        }
    }
    
    public void PlayAudio(string str, bool force = false)
    {
        audio.Play(str, force);
    }
    

    public void SetRotation(Vector3 r)
    {
        Quaternion.Euler(r);
        body.localEulerAngles = r;
    }
}
