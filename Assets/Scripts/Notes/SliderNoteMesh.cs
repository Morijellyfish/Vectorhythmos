using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderNoteMesh : MonoBehaviour
{
    [SerializeField] int line;
    [SerializeField] int size = 1;
    [SerializeField] bool EnableAnimationCurve;
    [SerializeField] public AnimationCurve Curve;
    [SerializeField] public float LandingTime;//着弾時刻
    [SerializeField] public LR type;
    public char direction;
    public enum LR
    {
        Left,Right
    }

    // Start is called before the first frame update
    void Start()
    {
        if (type == LR.Left)
        {
            direction = 'L';
        }
        if (type == LR.Right)
        {
            direction = 'R';
        }
    }

    // Update is called once per frame
    void Update()
    {
        GenerateMesh();
    }

    public void GenerateMesh()
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

        Mesh mesh = new Mesh();
        Vector3[] pos = new Vector3[2 + (size * 2)];
        int[] tras = new int[size * 6];

        //頂点の座標の生成
        var rad = Math.PI / 180;
        var angle = 180.0f / 16.0f;

        if (b >= 0)
        {
            pos[0] = b * 1.00f * new Vector2((float)Math.Cos(((angle * (line + 0)) + 180) * rad), (float)Math.Sin(((angle * (line + 0)) + 180) * rad)) + new Vector2(0, 5);
            pos[1] = b * 1.1f * new Vector2((float)Math.Cos(((angle * (line + 0)) + 180) * rad), (float)Math.Sin(((angle * (line + 0)) + 180) * rad)) + new Vector2(0, 5);
            for (int i = 0; i < size * 2; i += 2)
            {
                pos[2 + i] = b * 1.00f * new Vector2((float)Math.Cos(((angle * (line + 1 + (i / 2))) + 180) * rad), (float)Math.Sin(((angle * (line + 1 + (i / 2))) + 180) * rad)) + new Vector2(0, 5);
                pos[3 + i] = b * 1.1f * new Vector2((float)Math.Cos(((angle * (line + 1 + (i / 2))) + 180) * rad), (float)Math.Sin(((angle * (line + 1 + (i / 2))) + 180) * rad)) + new Vector2(0, 5);
            }
        }
        else
        {
            pos[0] = new Vector2(0, 5);
            pos[1] = new Vector2(0, 5);
            for (int i = 0; i < size * 2; i += 2)
            {
                pos[2 + i] = new Vector2(0, 5);
                pos[3 + i] = new Vector2(0, 5);
            }
        }

        mesh.vertices = pos;

        if (type == LR.Left)
        {
            //頂点の順序
            for (int i = 0; i < size; i++)
            {
                int x = i * 2;
                tras[i * 6 + 0] = x + 1;
                tras[i * 6 + 1] = x + 2;
                tras[i * 6 + 2] = x + 3;
            }
        }
        else
        {
            //頂点の順序
            for (int i = 0; i < size; i++)
            {
                int x = i * 2;
                tras[i * 3 + 0] = x + 0;
                tras[i * 3 + 1] = x + 3;
                tras[i * 3 + 2] = x + 1;
            }
        }
        mesh.triangles = tras;

        Color[] colors = new Color[mesh.vertices.Length];

        if (type == LR.Left)//orange
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(1f, 0.4f, 0.0f);
            }
            colors[0] = new Color(0.8f, 0.4f, 0.0f);
            colors[1] = new Color(0.8f, 0.4f, 0.0f);
            colors[size * 2] = new Color(0.8f, 0.4f, 0.0f);
            colors[size * 2 + 1] = new Color(0.8f, 0.4f, 0.0f);
        }
        else//green
        {
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(0.2f, 0.8f, 0.2f);
            }
            colors[0] = new Color(0.2f, 0.6f, 0.2f);
            colors[1] = new Color(0.2f, 0.6f, 0.2f);
            colors[size * 2] = new Color(0.2f, 0.6f, 0.2f);
            colors[size * 2 + 1] = new Color(0.2f, 0.6f, 0.2f);
        }
        mesh.colors = colors;

        GetComponent<MeshFilter>().mesh = mesh;


        Vector2[] cpos = new Vector2[pos.Length];
        for (int i = 0; i < cpos.Length; i += 2)
        {
            cpos[i / 2] = new Vector2(pos[i].x, pos[i].y);
            cpos[(i / 2) + (cpos.Length / 2)] = new Vector2(pos[(pos.Length - i) - 1].x, pos[(pos.Length - i) - 1].y);
        }
        PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
        polygon.pathCount = 1;
        polygon.SetPath(0, cpos);

    }

}
