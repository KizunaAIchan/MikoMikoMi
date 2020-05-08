using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentBase 
{
    //


    public abstract void Init(MikoChi mikochi);
    public abstract void Update(float deltatime);
    public abstract void Destory();
}
