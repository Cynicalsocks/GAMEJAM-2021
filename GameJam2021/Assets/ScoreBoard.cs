using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    #region Singleton
    public static ScoreBoard Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public float score = 0;
    [SerializeField] TextMeshProUGUI textMeshPro;

    private void Update()
    {
        DisplayScore();

        //check if we reached goal
    }

    void DisplayScore()
    {
        textMeshPro.text = "Score: " + score;
    }

    private void OnGUI()
    {
    }
}
