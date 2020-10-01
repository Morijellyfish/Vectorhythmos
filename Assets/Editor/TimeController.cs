using UnityEngine;
using UnityEditor;

public class TimeController : EditorWindow
{
    [MenuItem("Editor/TimeController")]
    static void init()
    {
        EditorWindow.GetWindow<TimeController>("TimeController");
    }
    private void OnGUI()
    {
        TimeManager.time = 0;
        var time_s = GameObject.Find("Scripts").GetComponent<TimeManager>().time_serialize;
        GameObject.Find("Scripts").GetComponent<TimeManager>().time_serialize = EditorGUILayout.FloatField("time", time_s);
        var objects = GameObject.FindGameObjectsWithTag("note");
        foreach (GameObject obj in objects)
        {
            var notem = obj.GetComponent<NoteMain>();
            obj.transform.GetChild(0).transform.localScale = new Vector2(notem.size_X, notem.size_Y) * (1 - notem.Curve.Evaluate((TimeManager.time+ time_s - notem.LandingTime) + 1));
            obj.transform.GetChild(0).transform.localPosition = new Vector2(0, notem.goal_Y - notem.Curve.Evaluate((TimeManager.time+ time_s - notem.LandingTime) + 1) * notem.goal_Y);
        }

        if (GUILayout.Button("Hello"))
        {
            Debug.Log("hi!");

        }
    }

}