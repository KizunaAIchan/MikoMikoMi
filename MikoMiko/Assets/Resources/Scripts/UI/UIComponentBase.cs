using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponentBase : MonoBehaviour
{
    public virtual void Init(object args)
    {

    }

    public virtual void Close()
    {
        Destroy(gameObject);
    }
}
