using UnityEngine;

public class DifficultyProgression
{
    private int startingButtonCount;
    private int currentButtonCount;
    private int buttonsToShow;

    public DifficultyProgression(int startingButtonCount)
    {
        this.startingButtonCount = startingButtonCount;
        currentButtonCount = startingButtonCount;
        buttonsToShow = 1;
    }

    public int CurrentButtonCount => currentButtonCount;
    public int ButtonsToShow => buttonsToShow;

    public void IncreaseLevel()
    {
        int nextCount = GameConfig.Instance.CalculateNextButtonAmount(currentButtonCount);
        currentButtonCount = Mathf.Clamp(nextCount, 1, GameConfig.Instance.MaxButtonCount);
        buttonsToShow++;
    }

    public bool CanIncreaseLevel()
    {
        int nextCount = GameConfig.Instance.CalculateNextButtonAmount(currentButtonCount);
        return nextCount <= GameConfig.Instance.MaxButtonCount;
    }

    public void Reset()
    {
        currentButtonCount = startingButtonCount;
        buttonsToShow = 1;
    }
}
