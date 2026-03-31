using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ButtonSpawner buttonSpawner;
    [SerializeField] private ButtonsManager buttonsManager;

    private DifficultyProgression difficultyProgression;
    private bool waitingForButtonInit = false;

    private void OnEnable()
    {
        GameEvents.OnRoundCompleted += HandleRoundCompleted;
    }

    private void OnDisable()
    {
        GameEvents.OnRoundCompleted -= HandleRoundCompleted;
    }

    private void Start()
    {
        // Initialize all systems
        if (GameConfig.Instance == null)
        {
            Debug.LogError("GameConfig not found in scene! Please add GameConfig to the scene.");
            return;
        }

        difficultyProgression = new DifficultyProgression(GameConfig.Instance.StartingButtonCount);

        // Initialize spawner and create starting buttons
        buttonSpawner.Initialize(difficultyProgression.CurrentButtonCount);
    }

    private void Update()
    {
        // Handle new button initialization
        if (waitingForButtonInit)
        {
            buttonsManager.StartNewRound();
            waitingForButtonInit = false;
            return;
        }

        // Handle manual difficulty increase (Space key for testing)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IncreaseDifficulty();
        }
    }

    private void HandleRoundCompleted(bool won)
    {
        if (won)
        {
            IncreaseDifficulty();
        }
        else
        {
            ResetGameOnLoss();
        }
    }

    private void ResetGameOnLoss()
    {
        Debug.Log("Player lost! Resetting to starting difficulty...");

        difficultyProgression.Reset();

        // Update ButtonsManager with reset difficulty
        buttonsManager.SetButtonsToShow(difficultyProgression.ButtonsToShow);

        // Spawn fresh buttons at starting count
        buttonSpawner.Initialize(difficultyProgression.CurrentButtonCount);

        // Flag to start new round on next frame
        waitingForButtonInit = true;

        // Notify listeners of reset
        GameEvents.RaiseGameReset();
    }

    private void IncreaseDifficulty()
    {
        if (!difficultyProgression.CanIncreaseLevel())
        {
            Debug.Log("Maximum difficulty reached!");
            return;
        }

        int previousButtonCount = difficultyProgression.CurrentButtonCount;
        difficultyProgression.IncreaseLevel();
        int newButtonCount = difficultyProgression.CurrentButtonCount;

        // Update ButtonsManager with new difficulty
        buttonsManager.SetButtonsToShow(difficultyProgression.ButtonsToShow);

        // Spawn new buttons
        buttonSpawner.SpawnAndConfigureNextLevel(previousButtonCount);

        // Flag to refresh buttons on next frame
        waitingForButtonInit = true;

        // Notify listeners
        GameEvents.RaiseDifficultyIncreased(newButtonCount, difficultyProgression.ButtonsToShow);
    }
}
