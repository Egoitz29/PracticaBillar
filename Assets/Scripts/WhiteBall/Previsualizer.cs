using UnityEngine;

[RequireComponent(typeof(BolaFisica))]
public class BolaDisparoPreview : MonoBehaviour
{
    public float fuerzaDisparo = 10f;
    public float radioBola = 0.15f;
    public LayerMask capaBolas;
    public LayerMask capaParedes; // NUEVO

    private BolaFisica bola;
    private Camera cam;
    private LineRenderer lineRenderer;

    void Start()
    {
        bola = GetComponent<BolaFisica>();
        cam = Camera.main;

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 0;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.red;
    }

    void Update()
    {
        if (!GetComponent<BolaDisparo>().IsAiming)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        Vector2 mouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direccion = ((Vector2)transform.position - mouseWorld).normalized;
        float distancia = Vector2.Distance(transform.position, mouseWorld);
        Vector2 velocidadInicial = direccion * distancia * fuerzaDisparo;

        Vector2 origen = transform.position;

        // Primero comprobamos colisión con bola
        RaycastHit2D hitBola = Physics2D.CircleCast(origen, radioBola, direccion, 10f, capaBolas);

        if (hitBola.collider != null && hitBola.collider.gameObject != gameObject)
        {
            Transform bolaObjetivo = hitBola.collider.transform;
            Vector2 puntoImpacto = hitBola.point;

            Vector2 normal = ((Vector2)bolaObjetivo.position - puntoImpacto).normalized;
            Vector2 tangente = new Vector2(-normal.y, normal.x);

            float v1n = Vector2.Dot(velocidadInicial, normal);
            float v1t = Vector2.Dot(velocidadInicial, tangente);

            Vector2 velBlancaPost = v1t * tangente;
            Vector2 velObjetivoPost = v1n * normal;

            Vector2 postGolpeBlanca = puntoImpacto + velBlancaPost.normalized * 1.5f;
            Vector2 postGolpeObjetivo = (Vector2)bolaObjetivo.position + velObjetivoPost.normalized * 1.5f;

            lineRenderer.positionCount = 6;
            lineRenderer.SetPosition(0, origen);
            lineRenderer.SetPosition(1, puntoImpacto);

            lineRenderer.SetPosition(2, puntoImpacto);
            lineRenderer.SetPosition(3, postGolpeBlanca);

            lineRenderer.SetPosition(4, bolaObjetivo.position);
            lineRenderer.SetPosition(5, postGolpeObjetivo);
        }
        else
        {
            // Si no choca con bola, buscamos colisión con pared
            RaycastHit2D hitPared = Physics2D.CircleCast(origen, radioBola, direccion, 10f, capaParedes);

            if (hitPared.collider != null)
            {
                Vector2 puntoImpacto = hitPared.point;
                Vector2 normal = hitPared.normal;

                // Reflexión: R = D - 2(D·N)N
                Vector2 direccionRebote = Vector2.Reflect(direccion, normal);

                Vector2 puntoDespuesRebote = puntoImpacto + direccionRebote.normalized * 3f;

                lineRenderer.positionCount = 3;
                lineRenderer.SetPosition(0, origen);
                lineRenderer.SetPosition(1, puntoImpacto);
                lineRenderer.SetPosition(2, puntoDespuesRebote);
            }
            else
            {
                // Línea recta si no colisiona con nada
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, origen);
                lineRenderer.SetPosition(1, origen + direccion * 5f);
            }
        }
    }
}

