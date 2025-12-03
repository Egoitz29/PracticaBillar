using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColorGame2D : MonoBehaviour
{
    [Header("Prefab y contenedor")]
    [SerializeField] private GameObject squarePrefab;        // Debe tener SpriteRenderer
    [SerializeField] private Transform gridParent;           // Objeto vacío para agrupar

    [Header("Cuadrícula")]
    [SerializeField] private int columns = 5;
    [SerializeField] private int rows = 2;
    [SerializeField] private Vector2 cellSize = new Vector2(1.5f, 1.5f);
    [SerializeField] private Vector2 startPosition = new Vector2(-3.0f, 1.0f);

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
        // PRIORIDAD: TOUCH PRIMERO (cubre móvil Y Editor simulando touch con ratón)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Vector2 worldPos = mainCam.ScreenToWorldPoint(touch.position);
                TryHitAt(worldPos);
            }
        }
        // SOLO SI NO HAY TOUCH: RATÓN (para builds PC standalone)
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
                Debug.Log($"Click detectado en cuadrado índice: {colorSquare.index}"); // DEBUG EXTRA
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

        // Color base aleatorio (mejorado para variedad)
        baseColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);

        // Dificultad creciente
        float diff = Mathf.Max(0.01f, 0.09f - (currentRound - 1) * 0.007f);

        correctIndex = Random.Range(0, squares.Count);

        for (int i = 0; i < squares.Count; i++)
        {
            SpriteRenderer sr = squares[i].GetComponent<SpriteRenderer>();
            ColorSquare cs = squares[i].GetComponent<ColorSquare>();

            if (i == correctIndex)
            {
                // El cuadrado distinto (cambio muy sutil)
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

        Debug.Log($"Ronda {currentRound}/{totalRounds} - Cuadrado diferente en índice: {correctIndex}");
    }

    private void CheckAnswer(int index)
    {
        if (isGameOver) return;

        if (index == correctIndex)
        {
            Debug.Log("CORRECTO! Siguiente ronda");
            currentRound++;
            StartRound();
        }
        else
        {
            Debug.Log($"INCORRECTO! Clickaste {index} pero era {correctIndex}");
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
        Debug.Log("VICTORIA! Completaste las 10 rondas sin fallar!");

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

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector2 pos = startPosition + new Vector2(c * cellSize.x, -r * cellSize.y);

                GameObject go = Instantiate(squarePrefab, pos, Quaternion.identity, gridParent);
                go.transform.localScale = Vector3.one * 0.9f;

                // Aseguramos collider (un poco más grande = clicks perfectos)
                BoxCollider2D col = go.GetComponent<BoxCollider2D>();
                if (col == null) col = go.AddComponent<BoxCollider2D>();
                col.size = new Vector2(1.5f, 1.5f);

                // COMPONENTE CLAVE: índice directo
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

// COMPONENTE PEQUEÑO (NO LO BORRES)
public class ColorSquare : MonoBehaviour
{
    [HideInInspector] public int index;
}