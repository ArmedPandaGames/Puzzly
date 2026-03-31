using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [SerializeField] private int startingButtonCount = 4;
    [SerializeField] private int maxButtonCount = 36;
    [SerializeField] private float roundShowingTime = 5f;

    public static GameConfig Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public int StartingButtonCount => startingButtonCount;
    public int MaxButtonCount => maxButtonCount;
    public float RoundShowingTime => roundShowingTime;

    public int CalculateGridColumns(int buttonCount)
    {
        return (int)Mathf.Sqrt(buttonCount);
    }

    public int CalculateNextButtonAmount(int currentButtonCount)
    {
        int gridSize = CalculateGridColumns(currentButtonCount) + 1;
        return gridSize * gridSize;
    }
}
