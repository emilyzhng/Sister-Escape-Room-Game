
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    [Header("Snake UI")]
    public RectTransform segmentPrefab;
    public RectTransform playArea;

    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("Movement")]
    public Vector2Int direction = Vector2Int.right;
    public float speed = 5f;
    public float speedMultiplier = 1f;
    public int initialSize = 4;
    public bool moveThroughWalls = false;

    private readonly List<RectTransform> segments =
        new List<RectTransform>();

    private Vector2Int input;
    private float nextUpdate;
    private RectTransform headRect;
    private Vector2 cellSize;
    private bool isDying;

    public Vector2 CellSize => cellSize;
    public RectTransform HeadRect => headRect;

    private void Start()
    {
        headRect = GetComponent<RectTransform>();

        if (playArea == null)
            playArea = headRect.parent as RectTransform;

        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();

        cellSize = headRect.rect.size;

        if (cellSize.x <= 0 || cellSize.y <= 0)
            cellSize = new Vector2(200f, 200f);

        ResetState();
    }

    private void Update()
    {
        if (gameManager != null &&
            (!gameManager.gameStarted || gameManager.gameEnded))
            return;

        if (Keyboard.current == null)
            return;

        if (direction.x != 0)
        {
            if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame)
                input = Vector2Int.up;
            else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame)
                input = Vector2Int.down;
        }
        else
        {
            if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.rightArrowKey.wasPressedThisFrame)
                input = Vector2Int.right;
            else if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.leftArrowKey.wasPressedThisFrame)
                input = Vector2Int.left;
        }
    }

    private void FixedUpdate()
    {
        if (headRect == null || isDying)
            return;

        if (gameManager != null &&
            (!gameManager.gameStarted || gameManager.gameEnded))
            return;

        if (Time.time < nextUpdate)
            return;

        if (input != Vector2Int.zero)
        {
            if (input + direction != Vector2Int.zero)
                direction = input;

            input = Vector2Int.zero;
        }

        Vector2[] previousPositions = new Vector2[segments.Count];

        for (int i = 0; i < segments.Count; i++)
        {
            if (segments[i] != null)
                previousPositions[i] = segments[i].anchoredPosition;
        }

        headRect.anchoredPosition += new Vector2(
            direction.x * cellSize.x,
            direction.y * cellSize.y
        );

        for (int i = segments.Count - 1; i >= 1; i--)
        {
            if (segments[i] != null)
                segments[i].anchoredPosition =
                    previousPositions[i - 1];
        }

        CheckOverlaps();

        nextUpdate = Time.time +
            1f / Mathf.Max(0.01f, speed * speedMultiplier);
    }

    public void Grow()
    {
        if (segmentPrefab == null || segments.Count == 0)
        {
            return;
        }

        RectTransform tail = segments[segments.Count - 1];

        RectTransform segment = Instantiate(
            segmentPrefab,
            headRect.parent,
            false
        );

        ConfigureSegment(segment);

        segment.anchoredPosition = tail.anchoredPosition -
            new Vector2(
                direction.x * cellSize.x,
                direction.y * cellSize.y
            );

        segments.Add(segment);
    }

    private void ConfigureSegment(RectTransform segment)
    {
        segment.anchorMin = headRect.anchorMin;
        segment.anchorMax = headRect.anchorMax;
        segment.pivot = headRect.pivot;
        segment.localScale = Vector3.one;
        segment.localRotation = Quaternion.identity;
        segment.sizeDelta = headRect.sizeDelta;
    }

    public void ResetState()
    {
        if (headRect == null)
            headRect = GetComponent<RectTransform>();

        if (headRect == null)
            return;

        isDying = false;
        direction = Vector2Int.right;
        input = Vector2Int.zero;

        cellSize = headRect.rect.size;

        if (cellSize.x <= 0 || cellSize.y <= 0)
            cellSize = new Vector2(200f, 200f);

        nextUpdate = Time.time +
            1f / Mathf.Max(0.01f, speed * speedMultiplier);

        for (int i = 1; i < segments.Count; i++)
        {
            if (segments[i] != null)
                Destroy(segments[i].gameObject);
        }

        segments.Clear();

        headRect.anchoredPosition = Vector2.zero;
        segments.Add(headRect);

        for (int i = 1; i < initialSize; i++)
        {
            if (segmentPrefab == null)
            {
                return;
            }

            RectTransform segment = Instantiate(
                segmentPrefab,
                headRect.parent,
                false
            );

            ConfigureSegment(segment);

            segment.anchoredPosition =
                headRect.anchoredPosition -
                new Vector2(
                    direction.x * cellSize.x * i,
                    direction.y * cellSize.y * i
                );

            segments.Add(segment);
        }
    }

    public bool Occupies(int x, int y)
    {
        Vector2 target = new Vector2(
            x * cellSize.x,
            y * cellSize.y
        );

        foreach (RectTransform segment in segments)
        {
            if (segment != null &&
                Vector2.Distance(
                    segment.anchoredPosition,
                    target
                ) < 0.1f)
                return true;
        }

        return false;
    }

    private void Die()
    {
        if (isDying)
            return;

        isDying = true;

        if (gameManager != null)
            gameManager.ResetScore();

        ResetState();
    }

    private void CheckOverlaps()
    {
        if (playArea == null || isDying)
            return;

        Image[] images =
            playArea.GetComponentsInChildren<Image>(false);

        foreach (Image image in images)
        {
            if (image == null ||
                !image.gameObject.activeInHierarchy)
                continue;

            RectTransform other = image.rectTransform;

            if (IsPartOfSnake(other))
                continue;

            if (!Overlaps(headRect, other))
                continue;

            if (image.CompareTag("Food"))
            {
                Food food = image.GetComponent<Food>();

                if (gameManager != null)
                    gameManager.CollectFood();

                if (gameManager != null && gameManager.gameEnded)
                    return;

                Grow();

                if (food != null)
                {
                    food.RandomizePosition();
                }
                else
                {
                    image.gameObject.SetActive(false);
                }

                break;

            }
            else if (image.CompareTag("Wall"))
            {
                if (moveThroughWalls)
                    TraverseUIWall(other);
                else
                    Die();

                return;
            }
        }

        for (int i = 1; i < segments.Count; i++)
        {
            if (segments[i] != null &&
                Overlaps(headRect, segments[i]))
            {
                Die();
                return;
            }
        }
    }

    private bool IsPartOfSnake(RectTransform target)
    {
        foreach (RectTransform segment in segments)
        {
            if (segment == target)
                return true;
        }

        return false;
    }

    private bool Overlaps(
        RectTransform a,
        RectTransform b)
    {
        Vector3[] cornersA = new Vector3[4];
        Vector3[] cornersB = new Vector3[4];

        a.GetWorldCorners(cornersA);
        b.GetWorldCorners(cornersB);

        Vector2 minA = RectTransformUtility
            .WorldToScreenPoint(null, cornersA[0]);

        Vector2 maxA = RectTransformUtility
            .WorldToScreenPoint(null, cornersA[2]);

        Vector2 minB = RectTransformUtility
            .WorldToScreenPoint(null, cornersB[0]);

        Vector2 maxB = RectTransformUtility
            .WorldToScreenPoint(null, cornersB[2]);

        Rect rectA = Rect.MinMaxRect(
            minA.x, minA.y, maxA.x, maxA.y);

        Rect rectB = Rect.MinMaxRect(
            minB.x, minB.y, maxB.x, maxB.y);

        return rectA.Overlaps(rectB);
    }

    private void TraverseUIWall(RectTransform wall)
    {
        Vector2 position = headRect.anchoredPosition;

        if (direction.x != 0)
        {
            position.x = -wall.anchoredPosition.x +
                direction.x * cellSize.x;
        }
        else
        {
            position.y = -wall.anchoredPosition.y +
                direction.y * cellSize.y;
        }

        headRect.anchoredPosition = position;
    }
}
