using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopJokerManager : MonoBehaviour
{
    [Header("Referencias")]
    public JokerManager jokerManager;      // Tu GestorJoker de la escena
    public GameManager gameManager;        // Tu GameManager (maneja el oro, puntos, etc.)
    public Transform shopPanel;            // Contenedor horizontal de los comodines de la tienda
    public Transform inventoryPanel;       // Contenedor donde se muestran los comprados
    public GameObject botonPrefab;         // Prefab del botón o carta del comodín

    [Header("Catálogo")]
    public List<Joker> jokersDisponibles;  // ScriptableObjects de los comodines
    public List<int> precios;              // Precio de cada comodín (mismo orden que arriba)

    void Start()
    {
        CrearTienda();
    }

    // Crea los botones de comodines en la tienda
    void CrearTienda()
    {
        for (int i = 0; i < jokersDisponibles.Count; i++)
        {
            Joker j = jokersDisponibles[i];
            int precio = precios[i];

            // Instancia del prefab
            GameObject boton = Instantiate(botonPrefab, shopPanel);

            // Cambiar el texto del botón (usa TMP o UI normal)
            var txtTMP = boton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            if (txtTMP != null)
                txtTMP.text = $"{j.nombre}\n{precio} oro";
            else
            {
                var txtUI = boton.GetComponentInChildren<Text>();
                if (txtUI != null)
                    txtUI.text = $"{j.nombre}\n{precio} oro";
            }

            // Mostrar icono si existe
            var icon = boton.transform.Find("Icon")?.GetComponent<Image>();
            if (icon && j.icono)
                icon.sprite = j.icono;

            // Asigna el script de arrastre
            var drag = boton.AddComponent<DraggableJoker>();
            drag.jokerData = j;
            drag.precio = precio;
            drag.shop = this;

            Debug.Log($"🃏 Añadido {j.nombre} a la tienda (precio {precio})");
        }

        Debug.Log("🛒 Tienda creada con " + jokersDisponibles.Count + " comodines.");
    }

    // Compra tradicional (si se hace clic en botón)
    public void Comprar(Joker j, int precio)
    {
        if (gameManager.Oro < precio)
        {
            Debug.Log("❌ Oro insuficiente para " + j.nombre);
            return;
        }

        gameManager.Oro -= precio;
        jokerManager.AddJokerFinal(j);
        AñadirAlInventarioVisual(j);
        Debug.Log($"✅ Comprado {j.nombre}. Oro restante: {gameManager.Oro}");
    }

    // Compra mediante arrastre hacia el inventario
    public void ComprarPorArrastre(Joker j, int precio)
    {
        if (gameManager.Oro < precio)
        {
            Debug.Log("❌ Oro insuficiente para " + j.nombre);
            return;
        }

        gameManager.Oro -= precio;
        jokerManager.AddJokerFinal(j);
        AñadirAlInventarioVisual(j);
        Debug.Log($"🃏 Comprado arrastrando {j.nombre}. Oro restante: {gameManager.Oro}");
    }

    // Crea visualmente el icono del Joker comprado en el inventario
    public void AñadirAlInventarioVisual(Joker j)
    {
        GameObject nuevoIcono = new GameObject(j.nombre, typeof(RectTransform), typeof(Image));
        nuevoIcono.transform.SetParent(inventoryPanel, false);
        var img = nuevoIcono.GetComponent<Image>();
        img.sprite = j.icono;
        img.preserveAspect = true;
        img.rectTransform.sizeDelta = new Vector2(160, 160);
        Debug.Log($"🎴 {j.nombre} añadido al inventario visual.");
    }
}
