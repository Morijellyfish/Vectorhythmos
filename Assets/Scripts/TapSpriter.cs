﻿using UnityEngine;
using System;

public class TapSpriter : MonoBehaviour
{
    [SerializeField] int line;
    [SerializeField] int size=1;
    [SerializeField] bool EnableAnimationCurve;
    [SerializeField] public AnimationCurve Curve;
    [SerializeField] public float LandingTime;//着弾時刻

    private void Update()
    {
        WriteSprite();
    }

    public void WriteSprite()
    {
        if (size <= 0)
        {
            size = 0;
            Debug.Log("Size value is out of range " + this.name);
        }
        float b = 7.8f - ((LandingTime - TimeManager.time) * 7.8f);//最終地点を1とする
        if (EnableAnimationCurve)
        {
            b = Curve.Evaluate(TimeManager.time-LandingTime) * 7.8f;//最終地点を1とした曲線を参照する
        }
        //画面範囲外(負の値)なら作らない
        if (b < 0)
        {
            return;
        }
        SpriteRenderer spriteR = gameObject.GetComponent<SpriteRenderer>();
        Texture2D tex2d = new Texture2D(Screen.width, Screen.height);
        tex2d.name = "tex2d";
        spriteR.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f), 75);
        Sprite o = spriteR.sprite;
        o.name = "tmpsprite";
        Vector2[] sv = new Vector2[2+(size*2)];
        Vector2[] pos = new Vector2[sv.Length];
        UInt16[] tras = new UInt16[size*6];

        //頂点の座標の生成
        var rad = Math.PI / 180;
        var angle = 180.0f / 16.0f;
        pos[0] = b * 1.00f * new Vector2((float)Math.Cos(((angle * (line + 0)) + 180) * rad), (float)Math.Sin(((angle * (line + 0)) + 180) * rad)) + new Vector2(0, 5);
        pos[1] = b * 1.05f * new Vector2((float)Math.Cos(((angle * (line + 0)) + 180) * rad), (float)Math.Sin(((angle * (line + 0)) + 180) * rad)) + new Vector2(0, 5);
        for (int i = 0; i < size*2; i += 2)
        {
            pos[2 + i] = b * 1.00f * new Vector2((float)Math.Cos(((angle * (line + 1 + (i / 2))) + 180) * rad), (float)Math.Sin(((angle * (line + 1 + (i / 2))) + 180) * rad)) + new Vector2(0, 5);
            pos[3 + i] = b * 1.05f * new Vector2((float)Math.Cos(((angle * (line + 1 + (i / 2))) + 180) * rad), (float)Math.Sin(((angle * (line + 1 + (i / 2))) + 180) * rad)) + new Vector2(0, 5);
        }

        //頂点の座標の変換
        for (int i = 0; i < sv.Length; i++)
        {
            sv[i] = Camera.main.WorldToScreenPoint(new Vector2(pos[i].x, pos[i].y));
            sv[i] = new Vector2(Mathf.Clamp(sv[i].x, 0, tex2d.width), Mathf.Clamp(sv[i].y, 0, tex2d.height));
        }

        //頂点の順序
        for (UInt16 i = 0; i*3 < tras.Length-2; i++)
        {
            tras[i*3] = i;
            tras[i*3 + 1] = (UInt16)(i + 1);
            tras[i*3 + 2] = (UInt16)(i + 2);
        }
        o.OverrideGeometry(sv, tras);

        //colliderの頂点の順序
        Vector2[] cpos = new Vector2[pos.Length];
        for (int i=0; i < cpos.Length; i+=2)
        {
            cpos[i / 2] = pos[i];
            cpos[(i / 2)+(cpos.Length/2)] = pos[(pos.Length-i)-1];
        }
        PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
        polygon.pathCount = 1;
        polygon.SetPath(0, cpos);

        
    }

    public void WriteSprite_debug()
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
        
        Vector2[] sv = new Vector2[2 + (size * 2)];
        Vector2[] pos = new Vector2[sv.Length];
        UInt16[] tras = new UInt16[size * 6];
        
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
        
        //頂点の順序
        for (UInt16 i = 0; i * 3 < tras.Length - 2; i++)
        {
            tras[i * 3] = i;
            tras[i * 3 + 1] = (UInt16)(i + 1);
            tras[i * 3 + 2] = (UInt16)(i + 2);
        }

        //overridegeometryはeditor上では使えないため
        for(int i = 0; i < tras.Length - 2; i++)
        {
            Debug.DrawLine(pos[tras[i]], pos[tras[i + 1]], Color.red, 0.001f);
            Debug.DrawLine(pos[tras[i+1]], pos[tras[i + 2]], Color.red, 0.001f);
            Debug.DrawLine(pos[tras[i+2]], pos[tras[i]], Color.red, 0.001f);
        }
        

        //colliderの頂点の順序
        Vector2[] cpos = new Vector2[pos.Length];
        for (int i = 0; i < cpos.Length; i += 2)
        {
            cpos[i / 2] = pos[i];
            cpos[(i / 2) + (cpos.Length / 2)] = pos[(pos.Length - i) - 1];
        }
        PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
        polygon.pathCount = 1;
        polygon.SetPath(0, cpos);


    }


}