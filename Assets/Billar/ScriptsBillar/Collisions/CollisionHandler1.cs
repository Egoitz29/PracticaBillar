using UnityEngine;

// Asegura que este GameObject tenga el componente BolaFisica
[RequireComponent(typeof(BolaFisica))]
public class BolaCollisionHandlerBlanca : MonoBehaviour
{
    private BolaFisica bola; // Referencia al componente de física personalizado de la bola

    // Eventos públicos que otros scripts pueden usar para reaccionar a colisiones
    public System.Action OnBandaRebote; // Se lanza al rebotar en una banda
    public System.Action<GameObject> OnColisionConOtraBola; // Se lanza al colisionar con otra bola

    private Vector2 posicionAnterior; // Guarda la posición anterior para hacer un CircleCast

    void Start()
    {
        // Se obtiene el componente BolaFisica
        bola = GetComponent<BolaFisica>();
        if (bola == null)
        {
            Debug.LogError("BolaFisica no encontrado en " + gameObject.name);
        }

        // Se guarda la posición inicial de la bola
        posicionAnterior = bola.transform.position;
    }

    void FixedUpdate()
    {
        if (bola == null) return;

        // -------------------------------------------
        // DETECCIÓN Y RESOLUCIÓN DE COLISIÓN CON OTRAS BOLAS
        // -------------------------------------------
        foreach (var otraBola in FindObjectsOfType<BolaFisica>())
        {
            // Ignora la colisión consigo misma
            if (otraBola == bola) continue;

            // Se detecta colisión circular con otra bola
            if (CollisionFunctions.CircleToCircle1(
                bola.transform.position, bola.radio,
                otraBola.transform.position, otraBola.radio,
                out Vector2 contactDirection, out float contactMagnitude, out Vector2 contactPoint))
            {
                // Lanza el evento de colisión para que otros scripts reaccionen
                OnColisionConOtraBola?.Invoke(otraBola.gameObject);

                // Variables para la resolución física
                Vector2 pos1 = bola.transform.position;
                Vector2 pos2 = otraBola.transform.position;

                Vector2 v1 = bola.velocidad;
                Vector2 v2 = otraBola.velocidad;

                // Normal y tangente de la colisión
                Vector2 normal = (pos2 - pos1).normalized;
                Vector2 tangent = new Vector2(-normal.y, normal.x);

                // Proyecciones de la velocidad sobre la normal y la tangente
                float v1n = Vector2.Dot(v1, normal);
                float v1t = Vector2.Dot(v1, tangent);
                float v2n = Vector2.Dot(v2, normal);
                float v2t = Vector2.Dot(v2, tangent);

                // Factor de restitución (qué tanto rebota)
                float restitution = 0.98f;

                // Velocidades después del choque (intercambio elástico aproximado)
                float v1nAfter = v2n * restitution;
                float v2nAfter = v1n * restitution;

                // Reconstrucción de velocidades tras el rebote
                bola.velocidad = v1nAfter * normal + v1t * tangent;
                otraBola.velocidad = v2nAfter * normal + v2t * tangent;

                // Cálculo de solapamiento entre bolas
                float overlap = bola.radio + otraBola.radio - Vector2.Distance(pos1, pos2);

                // Velocidad media para usar en efectos de sonido
                float velocidadMedia = (v1.magnitude + v2.magnitude) / 2f;

                // Sonido de colisión (si tiene el componente asignado)
                var sonido1 = bola.GetComponent<BolaSonidoColision>();
                var sonido2 = otraBola.GetComponent<BolaSonidoColision>();

                if (sonido1 != null) sonido1.ReproducirSonidoColision(velocidadMedia);
                if (sonido2 != null) sonido2.ReproducirSonidoColision(velocidadMedia);

                // Separación física si hay solapamiento
                if (overlap > 0)
                {
                    Vector2 separation = normal * (overlap / 2f + 0.001f);
                    bola.transform.position -= (Vector3)separation;
                    otraBola.transform.position += (Vector3)separation;
                }
            }
        }

        // -------------------------------------------
        // DETECCIÓN Y RESOLUCIÓN DE COLISIÓN CON BANDAS
        // -------------------------------------------
        foreach (var obstaculo in GameObject.FindGameObjectsWithTag("Banda"))
        {
            BoxCollider2D box = obstaculo.GetComponent<BoxCollider2D>();
            if (box == null) continue;

            // Se hace un CircleCast desde la posición anterior a la nueva para detectar si cruza la banda
            RaycastHit2D hit = Physics2D.CircleCast(
                posicionAnterior,
                bola.radio,
                bola.transform.position - (Vector3)posicionAnterior,
                Vector2.Distance(posicionAnterior, bola.transform.position),
                LayerMask.GetMask("Banda") // Solo colisiona con objetos en la capa "Banda"
            );

            if (hit.collider != null)
            {
                Vector2 contactDir = hit.normal;         // Dirección del rebote (normal)
                Vector2 puntoColision = hit.point;       // Punto de contacto con la banda

                OnBandaRebote?.Invoke(); // Lanza evento para otros scripts

                // Ajusta la posición para que no se incruste en la banda
                bola.transform.position = puntoColision + contactDir * (bola.radio + 0.001f);

                // Refleja la velocidad respecto a la normal de la banda (rebote realista)
                bola.velocidad = Vector2.Reflect(bola.velocidad, contactDir) * 0.95f; // 5% de pérdida de energía
            }
        }

        // Guarda la posición actual para el próximo frame
        posicionAnterior = bola.transform.position;
    }
}
