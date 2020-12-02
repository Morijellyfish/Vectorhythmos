﻿using UnityEngine;
using System;

public class HoldSpriter : MonoBehaviour
{
    [SerializeField] Note[] points;

    [Serializable]
    public class Note
    {
        public int line;
        public int size=1;
        public bool EnableAnimationCurve;
        public AnimationCurve Curve;
        public float LandingTime;//着弾時刻
    }
    private void Update()
    {
        WriteSprite();
    }

    public void WriteSprite()
    {
        Vector2[] AllSv= { };
        UInt16[] AllTras= { };
        gameObject.GetComponent<PolygonCollider2D>().pathCount = points.Length;

        for (int p = 0; p < points.Length; p++)
        {
            int line=points[p].line;
            int size = points[p].size;
            bool EnableAnimationCurve= points[p].EnableAnimationCurve;
            AnimationCurve Curve= points[p].Curve;
            float LandingTime= points[p].LandingTime;

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

            //頂点の座標の変換
            for (int i = 0; i < sv.Length; i++)
            {
                sv[i] = Camera.main.WorldToScreenPoint(new Vector2(pos[i].x, pos[i].y));
                sv[i] = new Vector2(Mathf.Clamp(sv[i].x, 0, Screen.width), Mathf.Clamp(sv[i].y, 0, Screen.height));
            }

            //頂点の順序
            for (UInt16 i = 0; i * 3 < tras.Length - 2; i++)
            {
                tras[i * 3] = (UInt16)(i +AllSv.Length);
                tras[i * 3 + 1] = (UInt16)(i + 1+ AllSv.Length);
                tras[i * 3 + 2] = (UInt16)(i + 2+ AllSv.Length);
            }

            //colliderの頂点の順序
            Vector2[] cpos = new Vector2[pos.Length];
            for (int i = 0; i < cpos.Length; i += 2)
            {
                cpos[i / 2] = pos[i];
                cpos[(i / 2) + (cpos.Length / 2)] = pos[(cpos.Length - i) - 1];
            }
            PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
            polygon.SetPath(p, cpos);

            Array.Resize(ref AllSv, AllSv.Length + sv.Length);
            Array.Resize(ref AllTras, AllTras.Length + tras.Length);
            sv.CopyTo(AllSv, AllSv.Length - sv.Length);
            tras.CopyTo(AllTras, AllTras.Length - tras.Length);
        }

        int sum = 0;
        for(int p = 0; p+1 < points.Length; p++)
        {
            //下辺から上辺
            UInt16[] tras = new UInt16[3 * points[p].size];
            for (int i = 0; i < points[p].size; i++)
            {
                tras[0 + (3 * i)] = (UInt16)(sum+0+(i*2));
                tras[1 + (3 * i)] = (UInt16)(sum+2+(i*2));
                UInt16 tmp = (UInt16)(sum+(2 * (points[p].size + 1)) + 1 + (i * 2));
                if (tmp > sum + (2 * (points[p].size + 1)) + (2 * (points[p + 1].size + 1)))//次のノーツの頂点を通過しないために
                {
                    tras[2 + (3 * i)] = (UInt16)(-1+sum + (2 * (points[p].size + 1))+(2 * (points[p+1].size + 1)));
                }
                else
                {
                    tras[2 + (3 * i)] = tmp;
                }
            }
            Array.Resize(ref AllTras, AllTras.Length + tras.Length);
            tras.CopyTo(AllTras, AllTras.Length - tras.Length);
            sum += 2 * (points[p].size + 1);//ここまでの頂点数

            //上辺から下辺
            tras = new UInt16[3*points[p + 1].size];
            for(int i = 0; i < points[p + 1].size; i++)
            {
                tras[0 + (3 * i)] = (UInt16)(1 + (sum) + (i * 2));
                tras[1 + (3 * i)] = (UInt16)(3 + (sum) + (i * 2));
                UInt16 tmp = (UInt16)((sum-(2*(points[p].size+1)))+2 + (i * 2));
                if (tmp > sum-1)
                {
                    tras[2 + (3 * i)] = (UInt16)(sum-2);
                }
                else
                {
                    tras[2 + (3 * i)] = tmp;
                }
            }
            Array.Resize(ref AllTras, AllTras.Length + tras.Length);
            tras.CopyTo(AllTras, AllTras.Length - tras.Length);

        }
        
        SpriteRenderer spriteR = gameObject.GetComponent<SpriteRenderer>();
        Texture2D tex2d = new Texture2D(Screen.width, Screen.height);
        spriteR.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f), 75);
        Sprite o = spriteR.sprite;
        o.OverrideGeometry(AllSv, AllTras);

    }
}