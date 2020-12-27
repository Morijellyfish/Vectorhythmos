﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapNoteMesh : MonoBehaviour
{
    [SerializeField] int line;
    [SerializeField] int size = 1;
    [SerializeField] bool EnableAnimationCurve;
    [SerializeField] public AnimationCurve Curve;
    [SerializeField] public float LandingTime;//着弾時刻
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        WriteMesh();
    }

    public void WriteMesh()
    {
        if (size <= 0)
        {
            size = 0;
            Debug.Log("Size value is out of range " + this.name);
        }
        float b = 7.8f - ((LandingTime - TimeManager.time) * 7.8f);//最終地点を1とする
        if (EnableAnimationCurve)
        {
            b = Curve.Evaluate(TimeManager.time - LandingTime) * 7.8f;//最終地点を1とした曲線を参照する
        }
        //画面範囲外(負の値)なら作らない
        if (b < 0)
        {
            return;
        }
        Mesh mesh = new Mesh();
        Vector3[] pos = new Vector3[2 + (size * 2)];
        int[] tras = new int[size * 6];

        //頂点の座標の生成
        var rad = Math.PI / 180;
        var angle = 180.0f / 16.0f;
        pos[0] = b * 1.00f * new Vector2((float)Math.Cos(((angle * (line + 0)) + 180) * rad), (float)Math.Sin(((angle * (line + 0)) + 180) * rad)) + new Vector2(0, 5);
        pos[1] = b * 1.05f * new Vector2((float)Math.Cos(((angle * (line + 0)) + 180) * rad), (float)Math.Sin(((angle * (line + 0)) + 180) * rad)) + new Vector2(0, 5);
        for (int i = 0; i < size * 2; i += 2)
        {
            pos[2 + i] = b * 1.00f * new Vector2((float)Math.Cos(((angle * (line + 1 + (i / 2))) + 180) * rad), (float)Math.Sin(((angle * (line + 1 + (i / 2))) + 180) * rad)) + new Vector2(0, 5);
            pos[3 + i] = b * 1.05f * new Vector2((float)Math.Cos(((angle * (line + 1 + (i / 2))) + 180) * rad), (float)Math.Sin(((angle * (line + 1 + (i / 2))) + 180) * rad)) + new Vector2(0, 5);
        }
        mesh.vertices = pos;


        //頂点の順序
        for (int i = 0; i < size; i++)
        {
            int x = i * 2;
            tras[i * 6] = x;
            tras[i * 6 + 1] = x + 2;
            tras[i * 6 + 2] = x + 1;
            tras[i * 6 + 3] = x + 1;
            tras[i * 6 + 4] = x + 2;
            tras[i * 6 + 5] = x + 3;
        }
        mesh.triangles = tras;

        Color[] colors = new Color[mesh.vertices.Length];
        
        for(int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.red;
        }
        colors[0] = Color.black;
        colors[1] = Color.black;
        colors[size*2] = Color.black;
        colors[size*2+1] = Color.black;
        mesh.colors = colors;

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
