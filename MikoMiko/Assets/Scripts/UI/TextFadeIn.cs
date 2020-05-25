using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFadeIn : BaseMeshEffect
{
    public float EachEffectTime = 0.02f;
    public float UpdateMeshTime = 0.01f;
    public bool isRunning = false;

    private int currentWordIndex = 1;
    private float updateWordTime = 0;

    private Text text;
    private int updateNum;
    private int wordUpdateTimes;
    private float alphaOffset;



    public delegate void OnTextFadeInFinish();

    public OnTextFadeInFinish callback = null;

    protected override void Awake()
    {
        text = GetComponent<Text>();
    }



    void Update()
    {
        float currentTime = Time.realtimeSinceStartup;
        if (isRunning)
        {
            if (currentTime - updateWordTime > UpdateMeshTime)
            {
                updateWordTime = currentTime;
                if (updateNum == wordUpdateTimes)
                {
                    if (currentWordIndex < text.text.Length)
                    {
                        currentWordIndex++;
                        updateNum = 0;
                    }
                    else
                    {
                        Stop();
                        isRunning = false;

                        if (callback != null)
                            callback();
                    }
                }
                updateNum++;
                graphic.SetVerticesDirty();
            }
        }
    }

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive() || !isRunning)
            return;


        List<UIVertex> verts = new List<UIVertex>();
        vh.GetUIVertexStream(verts);
        vh.Clear();

        if (verts.Count == 0)
            return;
        int count = currentWordIndex * 6;

        SetColor(verts, currentWordIndex, updateNum);
        AddVert(vh, verts, count);
        AddTriangle(vh, count);
    }

    private void AddVert(VertexHelper vh, List<UIVertex> verts, int count)
    {
        for (int i = 0; i < count; i += 6)
        {
            if (i >= verts.Count)
                continue;
            var tl = verts[i + 0];
            var tr = verts[i + 1];
            var bl = verts[i + 4];
            var br = verts[i + 3];
            var ct = GetCenterVertex(verts[i + 0], verts[i + 1]);
            var cb = GetCenterVertex(verts[i + 3], verts[i + 4]);

            vh.AddVert(tl);
            vh.AddVert(tr);
            vh.AddVert(bl);
            vh.AddVert(br);
            vh.AddVert(ct);
            vh.AddVert(cb);
        }

    }

    private void AddTriangle(VertexHelper vh, int count)
    {
        for (int i = 0; i < count; i += 6)
        {
            vh.AddTriangle(i + 0, i + 4, i + 2);
            vh.AddTriangle(i + 4, i + 5, i + 2);
            vh.AddTriangle(i + 4, i + 1, i + 5);
            vh.AddTriangle(i + 1, i + 3, i + 5);
        }
    }

    protected void SetColor(List<UIVertex> verts, int currentWordIndex, int updateNum)
    {
        int halfTimes = wordUpdateTimes * 2 / 3;
        currentWordIndex = (currentWordIndex - 1) * 6;
        if (updateNum < halfTimes)
        {
            SetColor(verts, currentWordIndex + 1 - 6, alphaOffset, updateNum + halfTimes);
            SetColor(verts, currentWordIndex + 3 - 6, alphaOffset, updateNum + halfTimes);


            SetColor(verts, currentWordIndex + 1, 0, updateNum);
            SetColor(verts, currentWordIndex + 3, 0, updateNum);

            SetColor(verts, currentWordIndex + 0, alphaOffset, updateNum);
            SetColor(verts, currentWordIndex + 4, alphaOffset, updateNum);


        }
        else
        {
            SetColor(verts, currentWordIndex + 1, alphaOffset, updateNum - halfTimes);
            SetColor(verts, currentWordIndex + 3, alphaOffset, updateNum - halfTimes);
        }
    }

    private void SetColor(List<UIVertex> verts, int index, float alphaOffset, int updateNum)
    {
        if (index < 0 || index >= verts.Count)
            return;
        UIVertex vertex = verts[index];
        Color temp = vertex.color;
        temp.a = 0;
        temp.a += alphaOffset * updateNum;
        vertex.color = temp;
        verts[index] = vertex;
    }

    private UIVertex GetCenterVertex(UIVertex left, UIVertex right)
    {
        UIVertex center = new UIVertex();
        center.normal = (left.normal + right.normal) / 2;
        center.position = (left.position + right.position) / 2;
        center.tangent = (left.tangent + right.tangent) / 2;
        center.uv0 = (left.uv0 + right.uv0) / 2;
        center.uv1 = (left.uv1 + right.uv1) / 2;

        var color = Color.Lerp(left.color, right.color, 0.5f);
        center.color = color;
        return center;
    }


    public void Stop()
    {
        currentWordIndex = text.text.Length;
        updateNum = wordUpdateTimes;
    }


    public void ShowText()
    {
        ResetData();
        isRunning = true;
    }


    private void ResetData()
    {
        updateWordTime = Time.realtimeSinceStartup;
        currentWordIndex = 1;
        updateNum = 0;
        wordUpdateTimes = Mathf.CeilToInt(EachEffectTime / UpdateMeshTime);
        alphaOffset = 1.0f / wordUpdateTimes;
    }

    public void SetText(string str)
    {
        text.text = str;
    }


}

