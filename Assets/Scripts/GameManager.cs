using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int buttonCount = 0;
    [SerializeField] private int startingButtonCount = 4;
    [SerializeField] private int maxButtonCount = 36;
    [Space]
    [SerializeField] private ButtonSpawner buttonSpawner;
    [SerializeField] private ButtonsManager buttonsManager;

    private bool waitingForButtonInit = false;

    // Start is called before the first frame update
    void Start()
    {
        buttonSpawner.ControlGridColumnCount(startingButtonCount);
        buttonSpawner.SpawnButtonNTimes(buttonSpawner.buttonPrefab, buttonSpawner.grid, buttonCount, startingButtonCount);
        buttonCount = startingButtonCount;
    }

    // Update is called once per frame
    void Update()
    {
        // Skip this frame if we're waiting for newly spawned buttons to initialize
        if (waitingForButtonInit)
        {
            buttonsManager.RefreshAndStartNewRound();
            waitingForButtonInit = false;
            return;
        }

        //test
        if (Input.GetKeyDown(KeyCode.Space) || buttonsManager.wonLastRound)
        {
            buttonsManager.buttonsToShow++;
            buttonsManager.wonLastRound = false;
            int newButtonCount = buttonSpawner.CalculateNextButtonAmount(buttonCount);
            newButtonCount = Mathf.Clamp(newButtonCount, 1, maxButtonCount);
            
            if (newButtonCount > buttonCount)
            {
                buttonSpawner.ControlGridColumnCount(newButtonCount);
                buttonSpawner.SpawnButtonNTimes(buttonSpawner.buttonPrefab, buttonSpawner.grid, buttonCount, newButtonCount);
                buttonCount = newButtonCount;
                waitingForButtonInit = true;
            }
        }
    }
}
