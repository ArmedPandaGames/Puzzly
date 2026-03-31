using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
public class ButtonSpawner : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject grid;

    GridLayoutGroup gridLayoutGroup;


    public int CalculateNextButtonAmount(int n)
    {
        int l = (int)math.sqrt(n) + 1;
        return l * l;
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
