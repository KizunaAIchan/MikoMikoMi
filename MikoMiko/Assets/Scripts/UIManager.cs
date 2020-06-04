using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UINames
{
    public static string rightClickMenu = "RightClickMenu";
    public static string configPage = "UIConfig";
    public static string ConfigComponet = "ConfigComponet";
    public static string AVGDataComponet = "AVGDataComponent";
    public static string Dialog = "Dialog";
    public static string AvgEditor = "AVGEditor";
    public static string ChatBubble = "ChatBubble";
    public static string QuickJump = "QuickJump";
    public static string QuickJumpItem = "QuickJumpItem";
    public static string LoveBar = "LovePointBar";

}


public class UIManager : MonoBehaviour
{

    public Dictionary<int, CustomText> customTexts = new Dictionary<int, CustomText>();
    public Dictionary<string, UIBase> allAliveUi = new Dictionary<string, UIBase>();
    public Dictionary<string, UIBase> poolUi = new Dictionary<string, UIBase>();

    public static UIManager instance = null;
    public Transform root;
    public void Awake()
    {
        instance = this;
        allAliveUi.Clear();
        PreLoadUI<UI_Config>(UINames.configPage);
        PreLoadUI<UI_RightClickMenu>(UINames.rightClickMenu);
        PreLoadUI<ChatBubble>(UINames.ChatBubble);

        var m = UIManager.instance.ShowUI<UI_Config>(UINames.configPage);
        m.InitComponent();
        m.OnBtnClickQuit();
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

        if (!poolUi.TryGetValue(name, out ui))
        {
            ui = ResourcesManager.instance.CreateUIPrefab(name);
        }
        if (ui == null) {
            Debug.Log("Check it !!!!");
            return null;
        }
        ui.gameObject.SetActive(true);
        ui.transform.SetParent(root);
        ui.transform.position = Vector3.zero;
        T t = ui as T;
        ui.name = name;
        ui.uiName = name;
        allAliveUi.Add(name, t);
        return t;
    }

    public T CreateComponent<T>(string name, Transform parent) where T : UIComponentBase
    {
        UIComponentBase ui = null;

        ui = ResourcesManager.instance.CreateUIComponent(name);
        if (ui == null)
        {
            Debug.Log("Check it !!!!");
            return null;
        }
        ui.transform.SetParent(parent);
        T t = ui as T;
        ui.name = name;
        return t;
    }

    public void CloseUIByName(string name)
    {
        UIBase ui = null;
        if (!allAliveUi.TryGetValue(name, out ui)) return;
        allAliveUi.Remove(name);
        AddToPool(name, ui);
      //  Destroy(ui.gameObject);
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

    public bool IsAlive(string name)
    {
        return allAliveUi.ContainsKey(name);
    }

    public T GetAliveUI<T>(string name) where T : UIBase
    {
        UIBase ui = null;
        allAliveUi.TryGetValue(name, out ui);
        return ui as T;
    }

    public static void HideUI(Transform obj, bool hide)
    {
        if (obj.position.x > 3535 && !hide)
        {
            var pos = obj.position;
            pos.x -= 3535;
            obj.position = pos;
        }
        else if (obj.position.x < 3535 && hide)
        {
            var pos = obj.position;
            pos.x += 3535;
            obj.position = pos;
        }
    }


    public void AddToPool(string name, UIBase ui)
    {
        poolUi[name] = ui;
        ui.transform.position = new Vector3(3535, -3535, 3535);
        ui.gameObject.SetActive(false);
    }

    public void PreLoadUI<T>(string name) where T : UIBase
    {
        UIBase ui = null;

        if (!poolUi.TryGetValue(name, out ui))
        {
            ui = ResourcesManager.instance.CreateUIPrefab(name);
        }
        if (ui == null)
        {
            Debug.Log("Check it !!!!");
            return ;
        }
        AddToPool(name, ui);
    }
}
