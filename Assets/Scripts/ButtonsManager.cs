using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    [Min(1)] private int buttonsToShow = 1;

    [Space]
    [SerializeField] private List<Button> buttons = new List<Button>();
    private readonly HashSet<Button> chosenButtons = new HashSet<Button>();

    private float showingTimeLeft;
    private bool roundActive;
    private bool canGuess = false;


    private void Start()
    {
        RefreshButtonCollection();
        HideAllButtons();
        roundActive = false;
    }

    private void Update()
    {
        if (roundActive)
        {
            if (showingTimeLeft > 0f)
            {
                showingTimeLeft -= Time.deltaTime;
            }
            else
            {
                if (!canGuess)
                {
                    HideAllButtons();
                    canGuess = true;
                    MakeButtonsPressable(true);
                }

                int pressedCount = buttons.Count(button => button.isPressed);
                if (pressedCount == chosenButtons.Count)
                {
                    bool allCorrect = chosenButtons.All(button => button.isPressed);
                    if (allCorrect)
                    {
                        EndRound(true);
                    }
                    else
                    {
                        EndRound(false);
                    }
                }
            }
        }
        else
        {
            StartNewRound();
        }
    }

    public void SetButtonsToShow(int count)
    {
        buttonsToShow = count;
    }

    private void RefreshButtonCollection()
    {
        buttons = new List<Button>(FindObjectsOfType<Button>());
    }

    public void StartNewRound()
    {
        RefreshButtonCollection();
        if (buttons.Count == 0)
        {
            Debug.LogWarning("No buttons available to show in ButtonsManager.");
            return;
        }

        ResetAllButtons();
        MakeButtonsPressable(false);

        chosenButtons.Clear();
        canGuess = false;

        int targetCount = Mathf.Clamp(buttonsToShow, 1, buttons.Count);

        while (chosenButtons.Count < targetCount)
        {
            Button candidate = buttons[Random.Range(0, buttons.Count)];
            if (chosenButtons.Add(candidate))
            {
                candidate.chosen = true;
            }
        }

        foreach (Button button in buttons)
        {
            if (chosenButtons.Contains(button))
            {
                button.Show();
            }
            else
            {
                button.Hide();
            }
        }

        showingTimeLeft = GameConfig.Instance.RoundShowingTime;
        roundActive = true;
    }

    private void EndRound(bool won)
    {
        Debug.Log(won ? "You won!" : "You lost!");

        roundActive = false;
        canGuess = false;
        HideAllButtons();
        ResetAllButtons();

        // Emit event instead of using public flag
        GameEvents.RaiseRoundCompleted(won);
    }

    private void ResetAllButtons()
    {
        foreach (Button button in buttons)
        {
            button.ResetState();
        }
    }

    private void HideAllButtons()
    {
        foreach (Button button in buttons)
        {
            button.Hide();
            button.chosen = false;
        }
    }

    private void MakeButtonsPressable(bool pressable)
    {
        foreach (Button button in buttons)
        {
            button.canBePressed = pressable;
        }
    }
}
