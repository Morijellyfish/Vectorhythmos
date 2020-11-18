using UnityEngine;
using System;

public class Spriter : MonoBehaviour
{
    private SpriteRenderer spriteR;

    static Texture2D tex2d;
    static Vector3 pos;

    [SerializeField] int line;
    [SerializeField] float time;
    [SerializeField] int size;

    private void Update()
    {
        oha();
    }

    void oha()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        // Create a blank Texture and Sprite to override later on.
        tex2d = new Texture2D(Screen.width, Screen.height);
        spriteR.sprite = Sprite.Create(tex2d, new Rect(0, 0, tex2d.width, tex2d.height), new Vector2(0.5f, 0.5f), 75);

        Sprite o = spriteR.sprite;
        Vector2[] sv = new Vector2[2+(size*2)];
        Vector2[] pos = new Vector2[sv.Length];
        UInt16[] tras = new UInt16[size*6];

        var rad = Math.PI / 180;
        var angle = 180.0f / 16.0f;
        pos[0] = time * 1.00f * new Vector2((float)Math.Cos(((angle * (line + 0)) + 180) * rad), (float)Math.Sin(((angle * (line + 0)) + 180) * rad))+new Vector2(0,5);
        pos[1] = time * 1.05f * new Vector2((float)Math.Cos(((angle * (line + 0)) + 180) * rad), (float)Math.Sin(((angle * (line + 0)) + 180) * rad)) + new Vector2(0, 5);
        for (int i = 0; i < size*2; i += 2)
        {
            pos[2+i] = time * 1.00f * new Vector2((float)Math.Cos(((angle * (line + 1+(i/2))) + 180) * rad), (float)Math.Sin(((angle * (line + 1+(i/2))) + 180) * rad)) + new Vector2(0, 5);
            pos[3+i] = time * 1.05f * new Vector2((float)Math.Cos(((angle * (line + 1+(i/2))) + 180) * rad), (float)Math.Sin(((angle * (line + 1+(i/2))) + 180) * rad)) + new Vector2(0, 5);
        }
        
        for (UInt16 i = 0; i*3 < tras.Length-2; i++)
        {
            tras[i*3] = i;
            tras[i*3 + 1] = (UInt16)(i + 1);
            tras[i*3 + 2] = (UInt16)(i + 2);
        }

        for (int i = 0; i < sv.Length; i++)
        {
            sv[i] = Camera.main.WorldToScreenPoint(new Vector2(pos[i].x, pos[i].y));
            sv[i] = new Vector2(Mathf.Clamp(sv[i].x, 0, tex2d.width), Mathf.Clamp(sv[i].y, 0, tex2d.height));
        }
        o.OverrideGeometry(sv, tras);

        Vector2[] cpos = new Vector2[pos.Length];
        for (int i=0; i < cpos.Length; i+=2)
        {
            cpos[i / 2] = pos[i];
            cpos[(i / 2)+(cpos.Length/2)] = pos[(pos.Length-i)-1];
        }

        

        PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
        polygon.pathCount = 2;
        polygon.SetPath(0, cpos);

        
    }
}