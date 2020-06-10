using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceManager : MonoBehaviour
{
    [System.Serializable]
    public struct FaceInfo
    {
        public Texture2D face;
        public string faceName;

    }

    public static FaceManager instance = null;

    public Material faceMat;
    public List<FaceInfo> faceInfos;

    public Dictionary<string, FaceInfo> faceInfoMap;

    private void Awake()
    {
        instance = this;
        faceInfoMap = new Dictionary<string, FaceInfo>();
        for (int i=0; i< faceInfos.Count; ++i)
        {
            faceInfoMap[faceInfos[i].faceName] = faceInfos[i];
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

    public void ChangeFace(string name)
    {
        FaceInfo info;
        if (faceInfoMap.TryGetValue(name, out info))
            faceMat.mainTexture = info.face;
    }
}
