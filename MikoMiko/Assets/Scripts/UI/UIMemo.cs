using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UIMemo : UIBase
{

    Dictionary<string, MemoComponent> memoComponents = new Dictionary<string, MemoComponent>();


    public Transform memoNode;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitMemoComponent()
    {
        string path = ResourcesManager.instance.MemoPath;

        DirectoryInfo folder = null;
        if (!Directory.Exists(path))
        {
            folder = Directory.CreateDirectory(path);
        }
        else
        {
            folder = new DirectoryInfo(path);
        }

        var memos = folder.GetFiles("*.txt");

        for (int i=0; i< memos.Length; ++i)
        {
            var memo = UIManager.instance.CreateComponent<MemoComponent>(UINames.MemoComponent, memoNode);
            memo.InitLoadText(memos[i].Name);
            memoComponents[memos[i].Name] = memo;

        }

 

    }


    public void DelMemoComponent(string str)
    {
        MemoComponent memo = null;
        if (memoComponents.TryGetValue(str, out memo))
        {
            memoComponents.Remove(str);

            memo.Close();
        }
    }

    public void ClearAllMemoComponent()
    {
        foreach (var v in memoComponents)
        {
            v.Value.Close();

        }
        memoComponents.Clear();
    }

    public void AddMemoComponent()
    {
        string name = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ") + memoComponents.Count.ToString();
        var memo = UIManager.instance.CreateComponent<MemoComponent>(UINames.MemoComponent, memoNode);
        memo.InitLoadText(name);
        memoComponents[name] = memo;
    }
}



