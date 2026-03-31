using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GameObject grid;

    private GridLayoutGroup gridLayoutGroup;

    public void Initialize(int startingButtonCount)
    {
        if (buttonPrefab == null || grid == null)
        {
            Debug.LogError("ButtonSpawner: buttonPrefab or grid is not assigned!");
            return;
        }

        gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
        if (gridLayoutGroup == null)
        {
            Debug.LogError("ButtonSpawner: Grid does not have GridLayoutGroup component!");
            return;
        }

        ClearButtons();

        // Spawn initial buttons
        SpawnButtons(0, startingButtonCount);
        UpdateGridColumns(startingButtonCount);
    }

    public void SpawnAndConfigureNextLevel(int currentButtonCount)
    {
        int newButtonCount = GameConfig.Instance.CalculateNextButtonAmount(currentButtonCount);
        newButtonCount = Mathf.Clamp(newButtonCount, 1, GameConfig.Instance.MaxButtonCount);

        if (newButtonCount > currentButtonCount)
        {
            SpawnButtons(currentButtonCount, newButtonCount);
            UpdateGridColumns(newButtonCount);
        }
    }

    private void SpawnButtons(int fromIndex, int toIndex)
    {
        for (int i = fromIndex; i < toIndex; i++)
        {
            Instantiate(buttonPrefab, grid.transform);
        }
    }

    private void UpdateGridColumns(int buttonCount)
    {
        int columnCount = GameConfig.Instance.CalculateGridColumns(buttonCount);
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columnCount;
    }

    private void ClearButtons()
    {
        for (int i = grid.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(grid.transform.GetChild(i).gameObject);
        }
    }

    // Kept for backwards compatibility if needed
    public int CalculateNextButtonAmount(int n)
    {
        return GameConfig.Instance.CalculateNextButtonAmount(n);
    }

    public void SpawnButtonNTimes(GameObject button, GameObject parent, int n, int newN)
    {
        for (int i = n; i < newN; i++)
        {
            Instantiate(button, parent.transform);
        }
    }

    public void ControlGridColumnCount(int n)
    {
        gridLayoutGroup = grid.GetComponent<GridLayoutGroup>();
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = (int)math.sqrt(n);
    }
}
