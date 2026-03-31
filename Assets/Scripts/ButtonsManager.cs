using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField, Min(0f)] private float showingTime = 5f;
    [Min(1)] public int buttonsToShow = 1;

    [Space]
    [SerializeField] private List<Button> buttons = new List<Button>();
    private readonly HashSet<Button> chosenButtons = new HashSet<Button>();

    private float showingTimeLeft = 5f;
    private bool roundActive;
    private bool canGuess = false;
    public bool wonLastRound = false;


    private void Start()
    {
        RefreshButtonCollection();
        HideAllButtons();
        ResetRoundTimer();
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
                        EndRound(true, "All correct buttons pressed");
                        wonLastRound = true;

                    }
                    else
                    {
                        EndRound(false, "Incorrect buttons pressed");
                        wonLastRound = false;
                    }
                }
            }
        }
        else
        {
            StartNewRound();
        }
    }

    public void RefreshAndStartNewRound()
    {
        StartNewRound();
    }

    private void RefreshButtonCollection()
    {
        buttons = new List<Button>(FindObjectsOfType<Button>());
    }

    private void StartNewRound()
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

        showingTimeLeft = showingTime;
        roundActive = true;

    }

    private void EndRound(bool won, string reason)
    {
        Debug.Log(won ? $"You won! ({reason})" : $"You lost! ({reason})");

        foreach (Button button in buttons)
        {
            button.chosen = false;
        }

        roundActive = false;
        canGuess = false;
        HideAllButtons();
        ResetRoundTimer();
        ResetAllButtons();

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

    private void ResetRoundTimer()
    {
        showingTimeLeft = showingTime;
    }

    private void MakeButtonsPressable(bool pressable)
    {
        foreach (Button button in buttons)
        {
            button.canBePressed = pressable;
        }
    }
}
