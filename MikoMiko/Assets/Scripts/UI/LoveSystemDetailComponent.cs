using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoveSystemDetailComponent :UIComponentBase
{


    public Text audioCount;
    public Text audioTotalCount;

    public Text animaCount;
    public Text animaTotalCount;


    public Text loveLv;
    public Text lovePoint;
    public Text loveTotalPoint;

    public Image loveprocess;


    public Text TouchLimit;
    public Text dialogueLimit;
    public Text idleLimit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitComponent()
    {
        int love = MikoChi.instance.GetLove();
        int Lv = AVGDataManager.instance.GetCurrentLoveLevel(love);
        if (Lv == 999)
        {
            loveLv.text = "Max";
            loveprocess.fillAmount = 1;
            lovePoint.text = Tools.GetStringByNum(35353);
            loveTotalPoint.text = Tools.GetStringByNum(35353);
        }
        else
        {
            var config = AVGDataManager.instance.GetLoveConfigByLv(Lv);
            loveLv.text = Tools.GetStringByNum(Lv);

            loveprocess.fillAmount = (float)(love - (config.totalPoint - config.lovePoint)) / (float)config.lovePoint;

            lovePoint.text = Tools.GetStringByNum((love - (config.totalPoint - config.lovePoint)));
            loveTotalPoint.text = Tools.GetStringByNum(config.lovePoint);
        }

        int totalcount = ResourcesManager.instance.audioTotalCount;
        int audioCnt = ResourcesManager.instance.GetaudioClipList(true).Count;

        int animaTotalCount = MikoChi.instance.animationTimeList.Count;
        var list = MikoChi.instance.animationTimeList;
        int animaCnt = 0;
        for(int i=0; i<list.Count; ++i)
        {
            if (Lv >= list[i].requireLoveLv)
                animaCnt++;
        }

        audioCount.text = Tools.GetStringByNum(audioCnt);
        audioTotalCount.text = Tools.GetStringByNum(totalcount);

        this.animaCount.text = Tools.GetStringByNum(animaCnt);
        this.animaTotalCount.text = Tools.GetStringByNum(animaTotalCount);

        int limit = ResourcesManager.instance.GetLoveTypeCount(AddLoveType.Dialogue);
        dialogueLimit.text = Tools.GetStringByNum(limit);

        limit = ResourcesManager.instance.GetLoveTypeCount(AddLoveType.Touch);
        TouchLimit.text = Tools.GetStringByNum(limit);

        limit = ResourcesManager.instance.GetLoveTypeCount(AddLoveType.Idle);
        idleLimit.text = Tools.GetStringByNum(limit);



    }
}
