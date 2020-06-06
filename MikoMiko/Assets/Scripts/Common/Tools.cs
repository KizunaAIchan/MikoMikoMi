using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{

    public static Dictionary<int, string> numToString = new Dictionary<int, string>();


    public static string GetStringByNum(int num)
    {
        string str = "";
        if (!numToString.TryGetValue(num, out str))
            return num.ToString();
        return str;
    }


    public static void Init()
    {
         for (int i=0; i< 10000; ++i)
        {
            numToString[i] = i.ToString();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
