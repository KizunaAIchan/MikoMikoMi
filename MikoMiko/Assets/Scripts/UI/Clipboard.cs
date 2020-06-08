using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clipboard : UIBase
{


    public List<ClipboardComponent> comList;

    public Animation anima;
    public string lastClipContent = "";
    public List<string> copylist = new List<string>();
    void Start()
    {
       
    }

    public override void Init()
    {
        copylist.Clear();
        TimerManager.instance.AddTimer(0.5f, () =>
        {
            if (GUIUtility.systemCopyBuffer != null)
            {
                int cnt = GUIUtility.systemCopyBuffer.Length;
                string s = GUIUtility.systemCopyBuffer.ToString();
                if (GUIUtility.systemCopyBuffer.Length > 0)
                {
                    if (cnt != lastClipContent.Length || lastClipContent != s)
                    {
                        copylist.Add(s);
                        lastClipContent = s;
                        if (copylist.Count > 5)
                            copylist.RemoveAt(0);
                        RefreshComponent();
                    }
                }
            }


        }, true);
    }

    public void Show()
    {
        transform.localPosition = InitPos;
        anima.Play("clipbroad");
    }

    public void Hide()
    {
        UIManager.HideUI(this.transform, true);
    }

    public void RefreshComponent()
    {
        for (int i=0, jCnt = copylist.Count-1; i< comList.Count; ++i)
        {
            bool b = jCnt < 0;
            UIManager.HideUI(comList[i].transform, b);
            if (!b)
                comList[i].Init(copylist[jCnt]);
            --jCnt;
        }
    }

    public void Clear()
    {
        copylist.Clear();
        RefreshComponent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
