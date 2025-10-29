using UnityEngine;

// Este script se coloca en cada bola del jugador. 
// Se encarga de contar los rebotes que hace una bola ANTES de tocar la bola blanca,
// y proporciona datos al GameManager para calcular los puntos al final del turno.
public class ContadorRebotesAntesDeBlanca : MonoBehaviour
{
    public string tagBolaBlanca = "BolaBlanca"; // El tag que identifica a la bola blanca (para detección de colisión)

    // Cantidad de rebotes antes de tocar la bola blanca
    public int rebotes { get; set; } = 0;

    // True si la bola ha tocado la blanca (solo se marca una vez)
    public bool haTocadoBlanca { get; private set; } = false;

    // True si la bola se ha lanzado (es decir, si empezó a moverse)
    public bool haSidoLanzada { get; private set; } = false;

    private BolaFisica bolaFisica; // Referencia al script que controla el movimiento de esta bola

  
    // Se ejecuta al iniciar el juego o al activarse el objeto
    void Start()
    {
        // Busca el componente BolaFisica en la misma bola
        bolaFisica = GetComponent<BolaFisica>();

        // Si no lo encuentra, lanza un error y desactiva este script
        if (bolaFisica == null)
        {
            Debug.LogError("BolaFisica no encontrada en " + gameObject.name);
            enabled = false;
        }
    }

    // Se ejecuta una vez por frame
    void Update()
    {
        // Si la bola empieza a moverse por primera vez, se marca como lanzada
        if (bolaFisica.EstaEnMovimiento && !haSidoLanzada)
        {
            haSidoLanzada = true;
           
        }
        if (bolaFisica.EstaEnMovimiento)
        {
            Debug.Log("Rebotes:" + rebotes);
        }
      
    }

    // Llamado desde otro script (como BolaCollisionHandler) cuando se detecta un rebote
    public void ContarRebote()
    {
        // Solo cuenta rebotes si la bola ya ha sido lanzada y aún no ha tocado la blanca
        if (haSidoLanzada && !haTocadoBlanca)
            rebotes++;
    }

    // Llamado cuando esta bola colisiona con otra para verificar si es la blanca
    public void VerificarBolaBlanca(GameObject otraBola)
    {
        // Si aún no ha tocado la blanca y la bola con la que colisiona tiene el tag adecuado
        if (!haTocadoBlanca && otraBola.CompareTag(tagBolaBlanca))
            haTocadoBlanca = true; // Se marca como que ha tocado la bola blanca
    }

    // Devuelve true si la bola está completamente detenida (sin moverse)
    public bool EstaQuieto()
    {
        return !bolaFisica.EstaEnMovimiento;
    }

    // Reinicia todos los valores para que la bola esté lista para el próximo turno
    public void Resetear()
    {
        rebotes = 0;
        haTocadoBlanca = false;
        haSidoLanzada = false;
    }

   
}
