using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DifficultySelector : MonoBehaviour
{
    public Toggle[] toggles;
    public TMP_Text difficultyText;

    void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int capture = i;
            toggles[i].onValueChanged.AddListener((isSelected) => {
                if (isSelected)
                {
                    DifficultyManager.Instance.SetDifficulty((EnemyMeterManager.Difficulty)capture);
                    UpdateDifficultyText((EnemyMeterManager.Difficulty)capture);
                }
            });
        }
    }

    void UpdateDifficultyText(EnemyMeterManager.Difficulty difficulty)
    {
        difficultyText.text = "Selected Difficulty: " + difficulty.ToString();
    }
}