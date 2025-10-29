using UnityEngine;

[RequireComponent(typeof(BolaFisica))]
public class BolaCollisionHandler : MonoBehaviour
{
    public GameObject bola1;
    private BolaFisica bola;

    public GameObject floatingPointsPrefab;

    public System.Action OnBandaRebote;
    public System.Action<GameObject> OnColisionConOtraBola;

    private ContadorRebotesAntesDeBlanca contadorRebotesAntesDeBlanca;

    private Vector2 posicionAnterior;

    void Start()
    {
        contadorRebotesAntesDeBlanca = GetComponent<ContadorRebotesAntesDeBlanca>();
        if (contadorRebotesAntesDeBlanca == null)
        {
            Debug.LogError("ContadorRebotesAntesDeBlanca no encontrado en " + gameObject.name);
        }   

        bola = GetComponent<BolaFisica>();
        if (bola == null)
        {
            Debug.LogError("BolaFisica no encontrado en " + gameObject.name);
        }
        posicionAnterior = bola.transform.position;
    }

    void FixedUpdate()
    {
        if (bola == null) return;

        // Colisión con otras bolas
        foreach (var otraBola in FindObjectsOfType<BolaFisica>())
        {
            if (otraBola == bola) continue;

            if (CollisionFunctions.CircleToCircle1(
                bola.transform.position, bola.radio,
                otraBola.transform.position, otraBola.radio,
                out Vector2 contactDirection, out float contactMagnitude, out Vector2 contactPoint))
            {
                OnColisionConOtraBola?.Invoke(otraBola.gameObject);

                // Resolución física
                Vector2 pos1 = bola.transform.position;
                Vector2 pos2 = otraBola.transform.position;

                Vector2 v1 = bola.velocidad;
                Vector2 v2 = otraBola.velocidad;

                Vector2 normal = (pos2 - pos1).normalized;
                Vector2 tangent = new Vector2(-normal.y, normal.x);

                float v1n = Vector2.Dot(v1, normal);
                float v1t = Vector2.Dot(v1, tangent);
                float v2n = Vector2.Dot(v2, normal);
                float v2t = Vector2.Dot(v2, tangent);

                float restitution = 0.98f;

                float v1nAfter = v2n * restitution;
                float v2nAfter = v1n * restitution;

                bola.velocidad = v1nAfter * normal + v1t * tangent;
                otraBola.velocidad = v2nAfter * normal + v2t * tangent;

                float overlap = bola.radio + otraBola.radio - Vector2.Distance(pos1, pos2);

                float velocidadMedia = (v1.magnitude + v2.magnitude) / 2f;

                var sonido1 = bola.GetComponent<BolaSonidoColision>();
                var sonido2 = otraBola.GetComponent<BolaSonidoColision>();

                if (sonido1 != null) sonido1.ReproducirSonidoColision(velocidadMedia);
                if (sonido2 != null) sonido2.ReproducirSonidoColision(velocidadMedia);

                if (overlap > 0)
                {
                    Vector2 separation = normal * (overlap / 2f + 0.001f);
                    bola.transform.position -= (Vector3)separation;
                    otraBola.transform.position += (Vector3)separation;
                }

                Debug.Log("Colision");
                contadorRebotesAntesDeBlanca?.VerificarBolaBlanca(otraBola.gameObject);
            }
            
        }

        // Colisión con bandas
        foreach (var obstaculo in GameObject.FindGameObjectsWithTag("Banda"))
        {
            BoxCollider2D box = obstaculo.GetComponent<BoxCollider2D>();
            if (box == null) continue;

            RaycastHit2D hit = Physics2D.CircleCast(
                posicionAnterior,
                bola.radio,
                bola.transform.position - (Vector3)posicionAnterior,
                Vector2.Distance(posicionAnterior, bola.transform.position),
                LayerMask.GetMask("Banda") // Asegúrate de que las bandas estén en esta capa
            );

            if (hit.collider != null)
            {
                Vector2 contactDir = hit.normal;
                Vector2 puntoColision = hit.point;

                OnBandaRebote?.Invoke();

                // Rebote
                bola.transform.position = puntoColision + contactDir * (bola.radio + 0.001f);
                bola.velocidad = Vector2.Reflect(bola.velocidad, contactDir) * 0.95f;

                // Instanciar puntos flotantes
                if (floatingPointsPrefab != null)
                {
                    GameObject puntos = Instantiate(floatingPointsPrefab, puntoColision, Quaternion.identity);
                    var script = puntos.GetComponent<FloatingPoints>();
                    if (script != null)
                    {
                        script.Initialize(10); // o la puntuación correspondiente
                    }
                }

                var banda = hit.collider.GetComponent<Banda>();
                if (banda != null && contadorRebotesAntesDeBlanca != null)
                {
                    Debug.Log($"Colisión con banda: {banda.GetType().Name}");
                    banda.AplicarComodines(bola1);
                    banda.SumarPuntos(contadorRebotesAntesDeBlanca);
                }
            }

        }

        posicionAnterior = bola.transform.position;
    }

}
