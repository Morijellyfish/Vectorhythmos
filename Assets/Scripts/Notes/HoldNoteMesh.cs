using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldNoteMesh : MonoBehaviour
{
    [SerializeField] Note[] points;

    [Serializable]
    public class Note
    {
        public int line;
        public int size = 1;
        public bool EnableAnimationCurve;
        public AnimationCurve Curve;
        public float LandingTime;//着弾時刻
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GenerateMesh();
    }

    public void GenerateMesh()
    {
        Vector3[] AllPos = { };
        int[] AllTras = { };
        Color[] AllColors = { };

        for (int p = 0; p < points.Length; p++)
        {
            if (points[p].size <= 0)
            {
                points[p].size = 0;
                Debug.Log("Size value is out of range " + this.name);
            }
            float b = 7.8f - ((points[p].LandingTime - TimeManager.time) * 7.8f);//最終地点を1とする
            if (points[p].EnableAnimationCurve)
            {
                b = points[p].Curve.Evaluate(TimeManager.time - points[p].LandingTime) * 7.8f;//最終地点を1とした曲線を参照する
            }
            
            Vector3[] pos = new Vector3[2 + (points[p].size * 2)];
            int[] tras = new int[points[p].size * 6];

            //頂点の座標の生成
            var rad = Math.PI / 180;
            var angle = 180.0f / 16.0f;
            if (b >= 0)
            {
                pos[0] = b * 1.00f * new Vector2((float)Math.Cos(((angle * (points[p].line + 0)) + 180) * rad), (float)Math.Sin(((angle * (points[p].line + 0)) + 180) * rad)) + new Vector2(0, 5);
                pos[1] = b * 1.01f * new Vector2((float)Math.Cos(((angle * (points[p].line + 0)) + 180) * rad), (float)Math.Sin(((angle * (points[p].line + 0)) + 180) * rad)) + new Vector2(0, 5);
                for (int i = 0; i < points[p].size * 2; i += 2)
                {
                    pos[2 + i] = b * 1.00f * new Vector2((float)Math.Cos(((angle * (points[p].line + 1 + (i / 2))) + 180) * rad), (float)Math.Sin(((angle * (points[p].line + 1 + (i / 2))) + 180) * rad)) + new Vector2(0, 5);
                    pos[3 + i] = b * 1.01f * new Vector2((float)Math.Cos(((angle * (points[p].line + 1 + (i / 2))) + 180) * rad), (float)Math.Sin(((angle * (points[p].line + 1 + (i / 2))) + 180) * rad)) + new Vector2(0, 5);
                }
            }
            else
            {
                pos[0] = new Vector2(0, 5);
                pos[1] = new Vector2(0, 5);
                for (int i = 0; i < points[p].size * 2; i += 2)
                {
                    pos[2 + i] = new Vector2(0, 5);
                    pos[3 + i] = new Vector2(0, 5);
                }
            }
            
            
            //頂点の順序
            for (int i = 0; i < points[p].size; i++)
            {
                int x = i * 2;
                tras[i * 6] = x+AllPos.Length;
                tras[i * 6 + 1] = x + 2 + AllPos.Length;
                tras[i * 6 + 2] = x + 1 + AllPos.Length;
                tras[i * 6 + 3] = x + 1 + AllPos.Length;
                tras[i * 6 + 4] = x + 2 + AllPos.Length;
                tras[i * 6 + 5] = x + 3 + AllPos.Length;
            }

            Color[] colors = new Color[pos.Length];

            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = new Color(0.5f, 0.5f, 1);
            }
            colors[0] = Color.blue;
            colors[1] = Color.blue;
            colors[points[p].size * 2] = Color.blue;
            colors[points[p].size * 2 + 1] = Color.blue;
            


            Vector2[] cpos = new Vector2[pos.Length];
            for (int i = 0; i < cpos.Length; i += 2)
            {
                cpos[i / 2] = new Vector2(pos[i].x, pos[i].y);
                cpos[(i / 2) + (cpos.Length / 2)] = new Vector2(pos[(pos.Length - i) - 1].x, pos[(pos.Length - i) - 1].y);
            }
            PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
            polygon.pathCount = p+1;
            polygon.SetPath(p, cpos);
            
            Array.Resize(ref AllPos, AllPos.Length + pos.Length);
            Array.Resize(ref AllTras, AllTras.Length + tras.Length);
            Array.Resize(ref AllColors, AllColors.Length + colors.Length);
            pos.CopyTo(AllPos, AllPos.Length - pos.Length);
            tras.CopyTo(AllTras, AllTras.Length - tras.Length);
            colors.CopyTo(AllColors, AllColors.Length - colors.Length);
        }
        Mesh mesh = new Mesh();
        mesh.vertices = AllPos;
        mesh.triangles = AllTras;
        mesh.colors = AllColors;
        GetComponent<MeshFilter>().mesh = mesh;

        GenerateTrail(mesh);
        GenerateTrailCollider(mesh);
    }

    public void GenerateTrail(Mesh mesh)
    {
        int[] AllTras = mesh.triangles;
        int Vsum = 0;
        //▲
        for (int p = 0; p < points.Length-1; p++)
        {
            int[] tras = new int[points[p].size * 3];
            for (int i = 0; i < points[p].size; i++)
            {
                tras[i * 3] = (i * 2)+Vsum;
                if (points.Length - 1 != p)
                {
                    if (points[p + 1].size >= i)
                    {
                        tras[i * 3 + 1] = (((points[p].size + 1) * 2) + 1 + (i * 2)) + Vsum;
                    }
                    else
                    {
                        tras[i * 3 + 1] = (((points[p].size + 1) * 2) + 1 + (points[p+1].size * 2)) + Vsum;
                    }
                }
                tras[i * 3 + 2] = (i * 2 + 2)+Vsum;
            }
            Array.Resize(ref AllTras, AllTras.Length + tras.Length);
            tras.CopyTo(AllTras, AllTras.Length - tras.Length);
            Vsum += (points[p].size + 1) * 2;
        }


        //▼
        Vsum = (points[0].size+1)*2;
        for (int p = 1; p < points.Length; p++)
        {
            int[] tras = new int[points[p].size * 3];
            for (int i = 0; i < points[p].size; i++)
            {
                tras[i * 3] = (i * 2) + Vsum +1;
                tras[i * 3 + 1] = ((i * 2) + 2) + Vsum+1;
                if (points[p - 1].size > i)
                {
                    tras[i * 3 + 2] = (Vsum - ((points[p - 1].size + 1) * 2)) + (i + 1) * 2;
                }
                else
                {
                    tras[i * 3 + 2] = (Vsum - ((points[p - 1].size + 1) * 2)) + (points[p-1].size) * 2;
                }
            }
            Array.Resize(ref AllTras, AllTras.Length + tras.Length);
            tras.CopyTo(AllTras, AllTras.Length - tras.Length);
            Vsum += ((points[p].size + 1) * 2);
        }



        mesh.triangles = AllTras;
        GetComponent<MeshFilter>().mesh = mesh;

    }

    public void GenerateTrailCollider(Mesh mesh)
    {
        int Vsum = 2*(points[0].size+1);
        PolygonCollider2D polygon = gameObject.GetComponent<PolygonCollider2D>();
        for(int p = 1; p < points.Length; p++)
        {
            Vector2[] pos = new Vector2[points[p-1].size+points[p].size+2];
            int index = 0;
            for(int i = 0; i < points[p - 1].size + 1; i++)
            {
                Debug.Log(i * 2 + Vsum - (2 * (points[p - 1].size + 1)));
                pos[index] = mesh.vertices[i * 2 + Vsum - (2 * (points[p - 1].size + 1))];
                index++;
            }
            for(int i = 0; i < points[p].size + 1; i++)
            {
                Debug.Log("Vsum"+Vsum);
                Debug.Log(Vsum + (2*(points[p].size + 1))-1 - (i * 2));
                pos[index] = mesh.vertices[Vsum + (2 * (points[p].size + 1)) - 1 - (i * 2)];
                index++;
            }
            Vsum += 2 * (points[p].size + 1);
            polygon.pathCount++;
            polygon.SetPath(polygon.pathCount - 1, pos);
        }
    }
}
