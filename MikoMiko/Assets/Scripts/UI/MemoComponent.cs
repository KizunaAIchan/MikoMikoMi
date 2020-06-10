using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class MemoComponent : UIComponentBase
{

    public InputField filed;

    public string MemoName;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitLoadText(string name)
    {
        MemoName = name;
        string path = ResourcesManager.instance.MemoPath;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }


        string memo = path + "/" + MemoName;

        if (File.Exists(memo))
        {
            string content = File.ReadAllText(memo, Encoding.UTF8);
            filed.text = content;
        }
    }

    public void OnEndEdit()
    {

        string path = ResourcesManager.instance.MemoPath;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }


        string memo = path + "/" + MemoName;


        if (!File.Exists(memo))
            File.Create(memo).Dispose();
        File.WriteAllText(memo, filed.text);
        Debug.Log("dddddd");
    }

    public void onBtnClickDel()
    {
        string path = ResourcesManager.instance.MemoPath;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string memo = path + "/" + MemoName;

        if (File.Exists(memo))
            File.Delete(memo);
    }
}
