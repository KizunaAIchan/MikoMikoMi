using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILovePointBar : UIBase
{


    public Text lv;
    public Image loveprocess;
    public Text addPoint;
    public Animation anima;
    public float startTime = 0;
    private bool show = false;

    private int timerid = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        this.transform.localPosition = InitPos;
        TimerManager.instance.RemoveTimer(timerid);

        if (!show)
            anima.Play("lovepoint");

        show = true;
        timerid = TimerManager.instance.AddTimer(2, () =>
        {
            show = false;
            Close();
        });
    }

    public void AddPoint(int n, bool limit)
    {
        if (limit)
        {
            addPoint.text = "Limit";
        }
        else
        {
            addPoint.text = "+" + n.ToString();
        }
        anima.Play("addlovepoint");

    }

    public void RefreshProcess()
    {
        int love =  MikoChi.instance.GetLove();
        int Lv = AVGDataManager.instance.GetCurrentLoveLevel(love);
        if (Lv == 999)
        {
            lv.text = "Max";
            loveprocess.fillAmount = 1;
        }
        else
        {
            var config = AVGDataManager.instance.GetLoveConfigByLv(Lv);
            lv.text = Lv.ToString();

            loveprocess.fillAmount = (float)(love - (config.totalPoint - config.lovePoint)) / (float)config.lovePoint;
        }


    }
}
