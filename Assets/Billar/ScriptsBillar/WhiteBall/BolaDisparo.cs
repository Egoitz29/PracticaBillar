using UnityEngine;
using UnityEngine.EventSystems; // para detectar toques sobre UI

[RequireComponent(typeof(BolaFisica))]
public class BolaDisparo : MonoBehaviour
{
    private BolaFisica bola;
    private Vector2 startPointerPos;
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

        // --- PATH: Touch (mobile / tablet) ---
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // evitar interacción si el toque está sobre la UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                // si el toque está en la UI, cancelar posible drag
                if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended)
                {
                    isDragging = false;
                    IsAiming = false;
                }
            }
            else
            {
                Vector2 touchWorldPos = Camera.main.ScreenToWorldPoint(touch.position);

                if (touch.phase == TouchPhase.Began)
                {
                    Collider2D col = Physics2D.OverlapPoint(touchWorldPos);
                    if (col != null && col.gameObject == gameObject)
                    {
                        isDragging = true;
                        IsAiming = true;
                        startPointerPos = touchWorldPos;
                    }
                }
                else if (isDragging && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
                {
                    Vector2 currentPos = Camera.main.ScreenToWorldPoint(touch.position);
                    Debug.DrawLine(transform.position, currentPos, Color.red);
                }
                else if (isDragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
                {
                    FinishShot(Camera.main.ScreenToWorldPoint(touch.position));
                }
            }

            // si hay al menos un touch, no procesamos input de ratón en este frame
            return;
        }

        // --- PATH: Mouse (editor / PC) ---
        // evitar interacción si el cursor está sobre la UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            // no hacemos nada si está en UI
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D col = Physics2D.OverlapPoint(mouseWorldPos);
                if (col != null && col.gameObject == gameObject)
                {
                    isDragging = true;
                    IsAiming = true;
                    startPointerPos = mouseWorldPos;
                }
            }

            if (isDragging && Input.GetMouseButton(0))
            {
                Vector2 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.DrawLine(transform.position, currentMousePos, Color.red);
            }

            if (isDragging && Input.GetMouseButtonUp(0))
            {
                Vector2 releasePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                FinishShot(releasePos);
            }
        }
    }

    private void FinishShot(Vector2 releaseWorldPos)
    {
        isDragging = false;
        IsAiming = false;

        Vector2 bolaPos = transform.position;
        Vector2 mouseReleasePos = releaseWorldPos;

        Vector2 direccion = (bolaPos - mouseReleasePos).normalized;
        float distancia = Vector2.Distance(bolaPos, mouseReleasePos);

        bola.AplicarVelocidad(direccion * distancia * fuerzaDisparo);

        if (gameManager != null)
        {
            gameManager.tirosRestantes--;
            Debug.Log(gameManager.tirosRestantes + " tiros restantes.");
        }
    }
}
