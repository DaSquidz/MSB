using UnityEngine;
using TMPro;

public class EquationManager : MonoBehaviour
{
    public TextMeshProUGUI currentEquationTMP;
    public TextMeshProUGUI nextEquationTMP;
    public MeterManager meterManager;

    private int currentX;
    private int currentY;
    private int nextX;
    private int nextY;

    void Start()
    {
        GenerateStartingEquations();
        meterManager.OnCreatureSummoned += AdjustEquationsForSummonCost;
    }

    void Update()
    {
        HandleNumberInput(KeyCode.Alpha0, KeyCode.Keypad0, 0);
        HandleNumberInput(KeyCode.Alpha1, KeyCode.Keypad1, 1);
        HandleNumberInput(KeyCode.Alpha2, KeyCode.Keypad2, 2);
        HandleNumberInput(KeyCode.Alpha3, KeyCode.Keypad3, 3);
        HandleNumberInput(KeyCode.Alpha4, KeyCode.Keypad4, 4);
        HandleNumberInput(KeyCode.Alpha5, KeyCode.Keypad5, 5);
        HandleNumberInput(KeyCode.Alpha6, KeyCode.Keypad6, 6);
        HandleNumberInput(KeyCode.Alpha7, KeyCode.Keypad7, 7);
        HandleNumberInput(KeyCode.Alpha8, KeyCode.Keypad8, 8);
        HandleNumberInput(KeyCode.Alpha9, KeyCode.Keypad9, 9);
    }
    
    void OnDestroy()
    {
        meterManager.OnCreatureSummoned -= AdjustEquationsForSummonCost;
    }

    void HandleNumberInput(KeyCode topRowKey, KeyCode numpadKey, int number)
    {
        if (Input.GetKeyDown(topRowKey) || Input.GetKeyDown(numpadKey))
        {
            UserInput(number);
        }
    }

    void GenerateStartingEquations()
    {
        currentX = 0;
        int correctAnswer = Random.Range(1, 10);
        currentY = currentX + correctAnswer;
        currentEquationTMP.text = $"{currentX} + ? = {currentY}";

        nextX = currentY;
        int nextCorrectAnswer = Random.Range(1, 10);
        nextY = nextX + nextCorrectAnswer;
        nextEquationTMP.text = $"{nextX} + ? = {nextY}";
    }

    public void UserInput(int input)
    {
        if (currentX + input == currentY)
        {
            meterManager.UpdateMeter(input);
            ScrollEquations();
        }
        else
        {
            AdjustCurrentEquationForIncorrectAnswer(input);
        }
    }

    void ScrollEquations()
    {
        currentX = nextX;
        currentY = nextY;
        currentEquationTMP.text = nextEquationTMP.text;

        nextX = currentY;
        int nextCorrectAnswer = Random.Range(1, 10);
        nextY = nextX + nextCorrectAnswer;
        nextEquationTMP.text = $"{nextX} + ? = {nextY}";
    }

    void AdjustCurrentEquationForIncorrectAnswer(int wrongInput)
    {

        int maxDecrease = Mathf.FloorToInt(meterManager.currentMeterValue);
        int decreaseAmount = Mathf.Min(wrongInput, maxDecrease);

        meterManager.UpdateMeter(-decreaseAmount);

        currentX -= decreaseAmount;
        currentY -= decreaseAmount;
        nextX -= decreaseAmount;
        nextY -= decreaseAmount;

        currentEquationTMP.text = $"{currentX} + ? = {currentY}";
        nextEquationTMP.text = $"{nextX} + ? = {nextY}";
    }
    
    private void AdjustEquationsForSummonCost(int summonCost)
    {
        currentX -= summonCost;
        currentY -= summonCost;
        nextX -= summonCost;
        nextY -= summonCost;

        currentEquationTMP.text = $"{currentX} + ? = {currentY}";
        nextEquationTMP.text = $"{nextX} + ? = {nextY}";
    }
    
}