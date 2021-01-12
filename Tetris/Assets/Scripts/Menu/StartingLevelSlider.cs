using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartingLevelSlider : MonoBehaviour
{

    [SerializeField]
    private TetrisStats tetrisStats;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private TextMeshProUGUI startingLevelNumber;

    private void Start() => slider.onValueChanged.AddListener((float value) => OnValueChanged(value));

    private void OnValueChanged(float value)
    {
        int valueInt = Mathf.RoundToInt(value);
        tetrisStats.StartingLevel = valueInt;
        startingLevelNumber.text = valueInt.ToString();
    }

}
