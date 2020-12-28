﻿using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class TimeController : EditorWindow
{

    float time_s;

    [MenuItem("Editor/TimeController")]
    static void init()
    {
        GetWindow<TimeController>("TimeController");
    }
    private void OnGUI()
    {
        time_s = GameObject.Find("Scripts").GetComponent<TimeManager>().time_serialize;
        GameObject.Find("Scripts").GetComponent<TimeManager>().time_serialize = EditorGUILayout.FloatField("time", time_s);
    }
    private void Update()
    {
        if (!EditorApplication.isPaused&&!EditorApplication.isPlayingOrWillChangePlaymode)
        {
            TimeManager.time = time_s;
            var objs = GameObject.FindGameObjectsWithTag("note");
            foreach (var obj in objs)
            {
                if (obj.GetComponent<TapSpriter>())
                {
                    obj.GetComponent<TapSpriter>().WriteSprite_debug();
                }
                if (obj.GetComponent<HoldSpriter>())
                {
                    obj.GetComponent<HoldSpriter>().WriteSprite_debug();
                }
                if (obj.GetComponent<TapNoteMesh>())
                {
                    obj.GetComponent<TapNoteMesh>().GenerateMesh();
                }
            }
        }
    }

}