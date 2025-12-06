using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZoneTrigger : MonoBehaviour
{
    [Header("Configuración de la Zona")]
    public int zoneId = 0;                    // ¡IMPORTANTE! Pon 0, 1 o 1 o 2 en cada zona
    public string[] minigames;                // Escenas de minijuegos posibles

    [Header("Efectos Visuales")]
    public GameObject glowEffect;
    public GameObject interactButton;

    private bool playerInside = false;
    private Button buttonComponent;

    void Start()
    {
        // Desactivamos efectos al inicio
        if (glowEffect) glowEffect.SetActive(false);
        if (interactButton) interactButton.SetActive(false);

        // Cacheamos el botón para no buscarlo cada vez
        if (interactButton != null)
            buttonComponent = interactButton.GetComponent<Button>();

        // Si esta zona ya fue completada en una partida anterior (por si recargas escena), la desactivamos
        if (GameSessionManager.Instance != null && GameSessionManager.Instance.IsZoneCompleted(zoneId))
        {
            MarkAsCompleted();
        }
    }



void OnTriggerEnter(Collider other)
{
    if (!other.CompareTag("Player")) return;

    // Si ya completamos esta zona → no hacemos nada
    if (GameSessionManager.Instance.IsZoneCompleted(zoneId))
        return;

    playerInside = true;

    // Mostramos feedback visual
    if (glowEffect) glowEffect.SetActive(true);
    if (interactButton) interactButton.SetActive(true);

    // Configuramos el botón (solo una vez)
    if (buttonComponent != null)
    {
        buttonComponent.onClick.RemoveAllListeners(); // por seguridad
        buttonComponent.onClick.AddListener(EnterMinigame);
    }
}

void OnTriggerExit(Collider other)
{
    if (!other.CompareTag("Player")) return;

    playerInside = false;

    // Ocultamos feedback
    if (glowEffect) glowEffect.SetActive(false);
    if (interactButton) interactButton.SetActive(false);

    // Limpiamos el listener para evitar errores
    if (buttonComponent != null)
        buttonComponent.onClick.RemoveAllListeners();
}

void EnterMinigame()
{
    // Evitamos doble clic o spam
    if (!playerInside || GameSessionManager.Instance == null) return;

    // Intentamos registrar esta zona como completada
    bool success = GameSessionManager.Instance.TryCompleteZone(zoneId);

    if (success)
    {
        // ÉXITO: primera vez que entra → marcamos como completada visualmente
        MarkAsCompleted();

        // Elegimos minijuego aleatorio y cargamos
        string scene = minigames[Random.Range(0, minigames.Length)];
        SceneManager.LoadScene(scene);
    }
    else
    {
        // Ya estaba completada (por si alguien fuerza el botón)
        Debug.LogWarning($"Intento de repetir zona ${zoneId}");
        MarkAsCompleted(); // por si acaso
    }
}

// Se llama cuando la zona se completa por primera vez
private void MarkAsCompleted()
{
    // Desactivamos todo feedback visual permanentemente
    if (glowEffect) glowEffect.SetActive(false);
    if (interactButton) interactButton.SetActive(false);
    if (buttonComponent != null)
        buttonComponent.onClick.RemoveAllListeners();

    // Opcional: cambiar color, poner un tick, desactivar collider, etc.
    Collider col = GetComponent<Collider>();
    if (col != null) col.enabled = false;

    // Opcional: desactivar todo el GameObject
    // gameObject.SetActive(false);
}

// Útil para debug en el inspector
void OnValidate()
{
    if (zoneId < 0 || zoneId > 2)
    {
        Debug.LogWarning($"ZoneTrigger en {gameObject.name}: zoneId debe ser 0, 1 o 2", this);
    }
}
}
