using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TetrisStatsDisplay : MonoBehaviour
{

    [SerializeField]
    private TetrisStats tetrisStats;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI linesText;
    public TextMeshProUGUI levelText;

    private void OnEnable()
    {
        tetrisStats.ScoreChanged += UpdateScoreText;
        tetrisStats.LinesChanged += UpdateLinesText;
        tetrisStats.LevelChanged += UpdateLevelText;
    }

    private void UpdateScoreText(float score)
    {
        scoreText.text = score.ToString();
    }

    private void UpdateLinesText(int lines)
    {
        linesText.text = lines.ToString();
    }

    private void UpdateLevelText(int level)
    {
        levelText.text = level.ToString();
    }

    private void OnDisable()
    {
        tetrisStats.ScoreChanged -= UpdateScoreText;
        tetrisStats.LinesChanged -= UpdateLinesText;
        tetrisStats.LevelChanged -= UpdateLevelText;
    }

}
