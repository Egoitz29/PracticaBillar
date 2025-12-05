using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorGame : MonoBehaviour
{
    [Header("Prefab y contenedor")]
    [SerializeField] private GameObject squarePrefab;        // Debe tener SpriteRenderer
    [SerializeField] private Transform gridParent;           // Objeto vacío para agrupar

    [Header("Cuadrícula")]
    [SerializeField] private int columns = 5;
    [SerializeField] private int rows = 2;
    [SerializeField] private Vector2 cellSize = new Vector2(1.5f, 1.5f);

    [Header("Juego")]
    [SerializeField] private int totalRounds = 10;
    [SerializeField] private string loseSceneName = "Derrota";
    [SerializeField] private string winSceneName = "Victoria";

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI roundText;

    private int currentRound = 1;
    private int correctIndex;
    private Color baseColor;
    private readonly List<GameObject> squares = new List<GameObject>();
    private Camera mainCam;
    private bool isGameOver = false;

    private void Awake()
    {
        mainCam = Camera.main;

        if (squarePrefab == null) Debug.LogError("[ColorGame2D] Falta asignar squarePrefab.");
        if (gridParent == null) Debug.LogError("[ColorGame2D] Falta asignar gridParent.");
    }

    private void Start()
    {
        if (squarePrefab == null || gridParent == null) return;

        currentRound = 1;
        isGameOver = false;
        StartRound();
    }

    private void Update()
    {
        if (isGameOver) return;
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 worldPos = mainCam.ScreenToWorldPoint(touch.position);
                TryHitAt(worldPos);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            TryHitAt(worldPos);
        }
    }

    private void TryHitAt(Vector2 worldPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);
        if (hit.collider != null && !isGameOver)
        {
            ColorSquare colorSquare = hit.collider.GetComponent<ColorSquare>();
            if (colorSquare != null)
            {
                CheckAnswer(colorSquare.index);
            }
        }
    }

    private void StartRound()
    {
        if (currentRound > totalRounds)
        {
            OnWin();
            return;
        }

        UpdateRoundText();
        ClearSquares();
        GenerateGrid();

        baseColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        float diff = Mathf.Max(0.01f, 0.09f - (currentRound - 1) * 0.007f);

        correctIndex = Random.Range(0, squares.Count);

        for (int i = 0; i < squares.Count; i++)
        {
            SpriteRenderer sr = squares[i].GetComponent<SpriteRenderer>();

            if (i == correctIndex)
            {
                sr.color = new Color(
                    Mathf.Clamp01(baseColor.r + diff),
                    Mathf.Clamp01(baseColor.g - diff * 0.6f),
                    Mathf.Clamp01(baseColor.b + diff * 0.4f)
                );
            }
            else
            {
                sr.color = baseColor;
            }
        }
    }

    private void CheckAnswer(int index)
    {
        if (isGameOver) return;

        if (index == correctIndex)
        {
            currentRound++;
            StartRound();
        }
        else
        {
            OnLose();
        }
    }

    private void OnLose()
    {
        isGameOver = true;
        ClearSquares();

        if (!string.IsNullOrEmpty(loseSceneName))
            SceneManager.LoadScene(loseSceneName);
    }

    private void OnWin()
    {
        isGameOver = true;
        ClearSquares();

        if (!string.IsNullOrEmpty(winSceneName))
            SceneManager.LoadScene(winSceneName);
    }

    private void UpdateRoundText()
    {
        if (roundText != null)
            roundText.text = $"Ronda {currentRound} / {totalRounds}";
    }

    private void GenerateGrid()
    {
        int index = 0;

        // Calculamos startPosition para centrar la grilla en la cámara
        Vector2 startPosition = new Vector2(
            -((columns - 1) * cellSize.x) / 2f,
            ((rows - 1) * cellSize.y) / 2f
        );

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector2 pos2D = startPosition + new Vector2(c * cellSize.x, -r * cellSize.y);
                Vector3 pos = new Vector3(pos2D.x, pos2D.y, 0f); // Z = 0

                GameObject go = Instantiate(squarePrefab, pos, Quaternion.identity, gridParent);
                go.transform.localScale = Vector3.one * 0.9f;

                BoxCollider2D col = go.GetComponent<BoxCollider2D>();
                if (col == null) col = go.AddComponent<BoxCollider2D>();
                col.size = new Vector2(1.5f, 1.5f);

                ColorSquare colorSquare = go.AddComponent<ColorSquare>();
                colorSquare.index = index;

                squares.Add(go);
                index++;
            }
        }
    }

    private void ClearSquares()
    {
        foreach (GameObject sq in squares)
        {
            if (sq != null)
                Destroy(sq);
        }
        squares.Clear();
    }
}

public class ColorSquare : MonoBehaviour
{
    [HideInInspector] public int index;
}
