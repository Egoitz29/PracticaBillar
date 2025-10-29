using UnityEngine;

[RequireComponent(typeof(BolaFisica))]
public class BolaDisparo : MonoBehaviour
{
    private BolaFisica bola;
    private Vector2 startMousePos;
    private bool isDragging = false;

    public float fuerzaDisparo = 10f;

    public bool IsAiming { get; private set; } = false;

    private GameManager gameManager;


    void Start()
    {
        bola = GetComponent<BolaFisica>();
        gameManager = GameManager.Instance;

        if (gameManager == null)
        {
            Debug.LogError("GameManager no encontrado");
        }

    }

    void Update()
    {
        // Si está en movimiento, no permitir disparo
        if (bola.EstaEnMovimiento)
        {
            isDragging = false;
            IsAiming = false;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D col = Physics2D.OverlapPoint(mouseWorldPos);
            if (col != null && col.gameObject == gameObject)
            {
                isDragging = true;
                IsAiming = true;
                startMousePos = mouseWorldPos;
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawLine(transform.position, currentMousePos, Color.red);
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            IsAiming = false;

            Vector2 bolaPos = transform.position;
            Vector2 mouseReleasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 direccion = (bolaPos - mouseReleasePos).normalized;
            float distancia = Vector2.Distance(bolaPos, mouseReleasePos);

            bola.AplicarVelocidad(direccion * distancia * fuerzaDisparo);

            gameManager.tirosRestantes--;   

            Debug.Log(gameManager.tirosRestantes + " tiros restantes.");

        }
    }
}


