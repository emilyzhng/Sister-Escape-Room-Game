
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Food : MonoBehaviour
{
    [Header("UI References")]
    public RectTransform playArea;

    [Header("Grid Settings")]
    public float cellSize = 200f;

    private RectTransform foodRect;
    private Snake snake;

    private void Awake()
    {
        foodRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (playArea == null)
            playArea = foodRect.parent as RectTransform;

        snake = FindFirstObjectByType<Snake>();

        RandomizePosition();
    }

    public void RandomizePosition()
    {
        if (playArea == null || foodRect == null)
        {
            return;
        }

        if (snake == null)
            snake = FindFirstObjectByType<Snake>();

        if (cellSize <= 0f)
        {
            return;
        }

        Rect area = playArea.rect;

        int minX = Mathf.CeilToInt(area.xMin / cellSize);
        int maxX = Mathf.FloorToInt(area.xMax / cellSize);
        int minY = Mathf.CeilToInt(area.yMin / cellSize);
        int maxY = Mathf.FloorToInt(area.yMax / cellSize);

        for (int attempts = 0; attempts < 1000; attempts++)
        {
            int x = Random.Range(minX, maxX + 1);
            int y = Random.Range(minY, maxY + 1);

            if (snake != null && snake.Occupies(x, y))
                continue;

            foodRect.anchoredPosition = new Vector2(
                x * cellSize,
                y * cellSize
            );

            gameObject.SetActive(true);
            return;
        }
    }
}
