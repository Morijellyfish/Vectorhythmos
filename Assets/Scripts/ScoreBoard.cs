using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] Text Score;
    [SerializeField] Text Title;
    [SerializeField] Text Difficulty;
    [SerializeField] Text Combo;

    static int score=0;
    static int combo=0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Score.text = "Score:"+score.ToString().PadLeft(7,'0');
        Combo.text = combo.ToString();
    }
}
