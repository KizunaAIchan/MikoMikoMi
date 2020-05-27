using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuickJump : UIBase
{

    public Transform content;

    public Transform node;
    public Dictionary<string, QuickJumpComponent> components = new Dictionary<string, QuickJumpComponent>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowComponentNode()
    {
        UIManager.HideUI(node, false);
    }

    public void HideComponentNode()
    {
        UIManager.HideUI(node, true);
    }


    public void InitComponent()
    {
        ClearComponent();

        var id = HttpRequest.instance.GetChannelsByStatus(LiveStatus.Streaming);

        for (int i=0; i<id.Count; ++i)
        {
            AddComponent(id[i]);
        }
    }


    public void AddComponent(string id)
    {
        QuickJumpComponent com = null;
        if (components.TryGetValue(id, out com))
        {
            return;
        }

        var config = ResourcesManager.instance.GetChannelConfigById(id);
        if (config == null) return;
        var component = UIManager.instance.CreateComponent<QuickJumpComponent>(UINames.QuickJumpItem, content);
        component.Init(config);
        components[id] = component;
    }

    public void RemoveComponent(string id)
    {
        QuickJumpComponent com = null;
        if (components.TryGetValue(id, out com))
        {
            components.Remove(id);
            com.Close();
        } 
    }

    public void onCloseUI()
    {
        ClearComponent();
        Close();
    }

    public void ClearComponent()
    {
        foreach(var item in components)
        {
            item.Value.Close();
        }

        components.Clear();
    }


}
