using UnityEngine;

public static class GameEvents
{
    public delegate void RoundCompletedHandler(bool won);
    public delegate void DifficultyIncreasedHandler(int newButtonCount, int buttonsToShow);
    public delegate void GameResetHandler();

    public static event RoundCompletedHandler OnRoundCompleted;
    public static event DifficultyIncreasedHandler OnDifficultyIncreased;
    public static event GameResetHandler OnGameReset;

    public static void RaiseRoundCompleted(bool won)
    {
        Debug.Log($"[GameEvents] Round {(won ? "Won" : "Lost")}");
        OnRoundCompleted?.Invoke(won);
    }

    public static void RaiseDifficultyIncreased(int newButtonCount, int buttonsToShow)
    {
        Debug.Log($"[GameEvents] Difficulty increased - Buttons: {newButtonCount}, To Show: {buttonsToShow}");
        OnDifficultyIncreased?.Invoke(newButtonCount, buttonsToShow);
    }

    public static void RaiseGameReset()
    {
        Debug.Log("[GameEvents] Game reset - Back to starting difficulty");
        OnGameReset?.Invoke();
    }
}
