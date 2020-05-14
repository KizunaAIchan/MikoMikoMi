using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UINames
{
    public static string rightClickMenu = "RightClickMenu";
}


public class UIManager : MonoBehaviour
{

    public Dictionary<int, CustomText> customTexts = new Dictionary<int, CustomText>();
    public Dictionary<string, UIBase> allAliveUi = new Dictionary<string, UIBase>();

    public static UIManager instance = null;
    public Transform root;
    public void Awake()
    {
        instance = this;
        allAliveUi.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public T ShowUI<T>(string name) where T:UIBase
    {
        UIBase ui = null;
        if (allAliveUi.TryGetValue(name, out ui))
            return ui as T;

        ui = ResourcesManager.instance.CreateUIPrefab(name);
        if (ui == null) {
            Debug.Log("Check it !!!!");
            return null;
        }
        ui.transform.SetParent(root);
        T t = ui as T;

        allAliveUi.Add(name, t);
        return t;
    }

    public void CloseUIByName(string name)
    {
        UIBase ui = null;
        if (!allAliveUi.TryGetValue(name, out ui)) return;
        allAliveUi.Remove(name);
        Destroy(ui.gameObject);
    }

    public void OnChangeLanguageSetting()
    {
        foreach(var txt in customTexts)
        {
            txt.Value.ChangeLanguageSetting();
        }
    }

    public void AddCustomText(CustomText txt)
    {
        customTexts[txt.GetInstanceID()] = txt;
    }
}
