using UnityEngine;

public class MenuDDOL : MonoBehaviour
{
    // Instancia estática para Singleton DDOL
    public static MenuDDOL Instance;

    private void Awake()
    {
        // Solo mantener una instancia
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // No destruir al cambiar de escena
        }
        else
        {
            Destroy(gameObject); // Destruir duplicados
        }
    }


}
