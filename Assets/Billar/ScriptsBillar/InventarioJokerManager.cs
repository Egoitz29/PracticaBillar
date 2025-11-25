using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventarioJokerManager : MonoBehaviour
{
    [Header("Referencias")]
    public Transform zonaInventario;          // Grid de slots
    public GameObject prefabJokerUI;          // Prefab de la carta del comodín
    public int maxJokers = 3;

    [SerializeField] private TMPro.TextMeshProUGUI mensajeAvisoTexto;
    private Coroutine avisoCoroutine;



    private List<Joker1> inventario = new List<Joker1>();

    public void ComprarJoker(Joker1 nuevoJoker)
    {
        if (nuevoJoker == null)
        {
            Debug.LogError("❌ ComprarJoker: nuevoJoker es NULL");
            return;
        }
        if (GameManager.Instance == null)
        {
            Debug.LogError("❌ ComprarJoker: GameManager.Instance es NULL");
            return;
        }
        if (zonaInventario == null || prefabJokerUI == null)
        {
            Debug.LogError("❌ Faltan referencias en InventarioJokerManager.");
            return;
        }

        // 🧹 Limpieza de nulls
        inventario.RemoveAll(j => j == null);

        // 🚫 Evitar duplicados
        if (inventario.Exists(j => j.nombre == nuevoJoker.nombre))
        {
            Debug.Log($"⚠️ Ya tienes el comodín '{nuevoJoker.nombre}' en el inventario.");
            MostrarAvisoTemporal($"⚠️ Ya tienes '{nuevoJoker.nombre}'");
            return;
        }

        // 📦 Comprobación de límite
        if (inventario.Count >= maxJokers)
        {
            Debug.Log("⚠️ Inventario lleno (máximo 3).");
            MostrarAvisoTemporal("⚠️ Inventario lleno");
            return;
        }

        // 💰 Comprobación de oro
        if (GameManager.Instance.Oro < nuevoJoker.precioCompra)
        {
            Debug.Log($"💰 Oro insuficiente: tienes {GameManager.Instance.Oro}, cuesta {nuevoJoker.precioCompra}");
            MostrarAvisoTemporal("💰 Oro insuficiente");
            return;
        }

        // 💰 Cobrar y añadir al inventario
        GameManager.Instance.Oro -= nuevoJoker.precioCompra;
        inventario.Add(nuevoJoker);

        // 🔍 Buscar el primer slot realmente libre (sin hijos activos)
        Transform slotLibre = null;
        int slotIndex = -1;

        for (int i = 0; i < zonaInventario.childCount; i++)
        {
            Transform slot = zonaInventario.GetChild(i);

            bool ocupado = false;
            foreach (Transform hijo in slot)
            {
                if (hijo.gameObject.activeInHierarchy)
                {
                    ocupado = true;
                    break;
                }
            }

            if (!ocupado)
            {
                slotLibre = slot;
                slotIndex = i;
                break;
            }
        }

        if (slotLibre == null)
        {
            Debug.Log("⚠️ No hay huecos libres en el inventario.");
            MostrarAvisoTemporal("⚠️ Inventario lleno");
            return;
        }

        // 🧱 Instanciar carta del comodín
        GameObject carta = Instantiate(prefabJokerUI, slotLibre);

        // Centrar visualmente
        RectTransform rt = carta.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one * 1.1f;
            rt.localRotation = Quaternion.identity;
        }

        // 🧾 Rellenar información visual
        Transform iconoT = carta.transform.Find("Icono");
        Image iconoImg = iconoT ? iconoT.GetComponent<Image>() : null;
        if (iconoImg != null) iconoImg.sprite = nuevoJoker.icono;

        var nombreText = carta.transform.Find("Nombre")?.GetComponent<TMPro.TextMeshProUGUI>();
        var descText = carta.transform.Find("Descripcion")?.GetComponent<TMPro.TextMeshProUGUI>();
        var precioText = carta.transform.Find("Precio")?.GetComponent<TMPro.TextMeshProUGUI>();

        if (nombreText) { nombreText.text = nuevoJoker.nombre; nombreText.gameObject.SetActive(false); }
        if (descText) { descText.text = nuevoJoker.descripcion; descText.gameObject.SetActive(false); }
        if (precioText) { precioText.text = "💰 " + nuevoJoker.precioVenta; precioText.gameObject.SetActive(false); }

        // 🎯 Componentes funcionales
        DraggableJokerInventario dragInv = carta.GetComponent<DraggableJokerInventario>();
        if (dragInv == null) dragInv = carta.AddComponent<DraggableJokerInventario>();
        dragInv.jokerData = nuevoJoker;

        ShowInfoJokerLocal showInfo = carta.GetComponent<ShowInfoJokerLocal>();
        if (showInfo == null) showInfo = carta.AddComponent<ShowInfoJokerLocal>();
        showInfo.joker = nuevoJoker;

        StartCoroutine(PopAnim(rt));

        // 💰 Actualizar UI de la tienda
        JokerShopUI shop = FindObjectOfType<JokerShopUI>();
        if (shop != null)
            shop.ActualizarOro(-nuevoJoker.precioCompra);

        Debug.Log($"✅ Comprado '{nuevoJoker.nombre}' en slot {slotIndex}. Oro actual: {GameManager.Instance.Oro}");

        // 🚀 Aplicar efecto directo del comodín
        nuevoJoker.AplicarEfecto(GameManager.Instance);
    }

    private void AplicarEfectoDirecto(Joker1 joker)
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || joker == null) return;

        switch (joker.tipoEfecto)
        {
            case Joker1.TipoEfecto.MasTiro:
                gm.AddTiros(1);
                Debug.Log("🎯 [+1 Tiro] aplicado al COMPRAR. Tiros ahora: " + gm.tirosRestantes);
                break;

            case Joker1.TipoEfecto.MenosMeta:
                gm.puntosRequeridos = Mathf.Max(1, gm.puntosRequeridos - 1);
                Debug.Log($"🎯 [Menos Meta] aplicado → Nueva meta: {gm.puntosRequeridos}");
                gm.uiManager?.ActualizarHUD();
                break;


            case Joker1.TipoEfecto.DoblePuntos:
               //m.MultiplicarPuntaje(2f);
                Debug.Log("💥 [Doble Puntos] aplicado al COMPRAR. Puntos: " + gm.puntosJugador);
                break;

            case Joker1.TipoEfecto.BolaFantasma:
                Debug.Log("👻 [Bola Fantasma] activado (pendiente de colisiones).");
                break;

            case Joker1.TipoEfecto.EscudoRebote:
                Debug.Log("🛡️ [Escudo de Rebote] activado (pendiente de lógica).");
                break;

            default:
                Debug.Log("ℹ️ [" + joker.nombre + "] sin efecto inmediato.");
                break;
        }

        gm.uiManager?.ActualizarHUD();
    }

    private void QuitarEfectoDirecto(Joker1 joker)
    {
        GameManager gm = GameManager.Instance;
        if (gm == null || joker == null) return;

        switch (joker.tipoEfecto)
        {
            case Joker1.TipoEfecto.MasTiro:
                gm.AddTiros(-1);
                Debug.Log("🎯 [-1 Tiro] al vender. Tiros ahora: " + gm.tirosRestantes);
                break;

            case Joker1.TipoEfecto.MenosMeta:
                gm.puntosRequeridos += 1;
                Debug.Log($"🎯 [Menos Meta] eliminado → Nueva meta: {gm.puntosRequeridos}");
                gm.uiManager?.ActualizarHUD();
                break;

            case Joker1.TipoEfecto.DoblePuntos:
                Debug.Log("💥 [Doble Puntos] desactivado al vender (pendiente de lógica).");
                break;

            case Joker1.TipoEfecto.BolaFantasma:
                Debug.Log("👻 [Bola Fantasma] desactivada (pendiente de lógica).");
                break;

            case Joker1.TipoEfecto.EscudoRebote:
                Debug.Log("🛡️ [Escudo de Rebote] desactivado (pendiente de lógica).");
                break;

            default:
                Debug.Log("ℹ️ [" + joker.nombre + "] sin efecto que revertir.");
                break;
        }

        gm.uiManager?.ActualizarHUD();
    }


    public void VenderJoker(Joker1 joker)
    {

        if (inventario.Contains(joker))
        {
            inventario.Remove(joker);
            joker.QuitarEfecto(GameManager.Instance);
            GameManager.Instance.Oro += joker.precioVenta;

            // 💰 Actualizar visualmente el oro en la tienda
            JokerShopUI shopUI = FindObjectOfType<JokerShopUI>();
            if (shopUI != null)
                shopUI.ActualizarOro(+joker.precioVenta);

            // 🎯 Actualizar también el HUD del juego (por si hay oro mostrado arriba)
            if (GameManager.Instance.uiManager != null)
                GameManager.Instance.uiManager.ActualizarHUD();

            Debug.Log($"💰 Vendido {joker.nombre}. Oro actual: {GameManager.Instance.Oro}");
        }

        if (joker == null) return;
        if (!inventario.Exists(j => j.nombre == joker.nombre)) return;

        inventario.RemoveAll(j => j.nombre == joker.nombre);
        GameManager.Instance.Oro += joker.precioVenta;

        foreach (Transform slot in zonaInventario)
        {
            foreach (Transform hijo in slot)
            {
                var drag = hijo.GetComponent<DraggableJokerInventario>();
                if (drag && drag.jokerData && drag.jokerData.nombre == joker.nombre)
                {
                    Destroy(hijo.gameObject);
                    break;
                }
            }
        }

        JokerShopUI shop = FindObjectOfType<JokerShopUI>();
        if (shop != null) shop.ActualizarOro(+joker.precioVenta);

        Debug.Log("🗑️ Vendido '" + joker.nombre + "'. Oro actual: " + GameManager.Instance.Oro);
        joker.QuitarEfecto(GameManager.Instance);


    }

    public List<Joker1> ObtenerInventario() => inventario;

    private IEnumerator PopAnim(RectTransform rt)
    {
        if (rt == null) yield break;

        Vector3 target = Vector3.one;
        float time = 0f;
        float duracion = 0.25f;

        while (time < duracion)
        {
            // 💥 Si el objeto fue destruido, sal inmediatamente
            if (rt == null || rt.gameObject == null)
                yield break;

            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / duracion);

            // Protegido contra referencias destruidas
            if (rt != null)
                rt.localScale = Vector3.Lerp(Vector3.one * 0.8f, target, t);

            yield return null;
        }

        // Última comprobación antes de aplicar la escala final
        if (rt != null)
            rt.localScale = target;
    }

    // 🔹 Aplica efectos pasivos como MenosMeta o MasTiro al inicio de cada ronda
    public void AplicarEfectosActivosAlInicio()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        foreach (var joker in inventario)
        {
            switch (joker.tipoEfecto)
            {
                case Joker1.TipoEfecto.MasTiro:
                    gm.AddTiros(1);
                    Debug.Log($"🎯 [{joker.nombre}] aplicado → +1 tiro (total: {gm.tirosRestantes})");
                    break;

                case Joker1.TipoEfecto.MenosMeta:
                    if (gm.puntosRequeridos > 1)
                    {
                        gm.puntosRequeridos -= 1;
                        Debug.Log($"🎯 [{joker.nombre}] aplicado → meta reducida a {gm.puntosRequeridos}");

                        // 💫 Efecto visual del HUD
                        gm.StartCoroutine(gm.uiManager.ParpadeoMetaVerde());
                    }
                    break;

                default:
                    break;
            }
        }

        gm.uiManager?.ActualizarHUD();
    }

    private void MostrarAvisoTemporal(string texto)
    {
        if (mensajeAvisoTexto == null) return;

        if (avisoCoroutine != null)
            StopCoroutine(avisoCoroutine);

        avisoCoroutine = StartCoroutine(AvisoCoroutine(texto));
    }

    private IEnumerator AvisoCoroutine(string texto)
    {
        mensajeAvisoTexto.text = texto;
        mensajeAvisoTexto.gameObject.SetActive(true);

        float tiempo = 0f;
        float duracion = 2f;

        // Parpadeo visual en rojo suave
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float alpha = Mathf.Abs(Mathf.Sin(tiempo * 6f)); // efecto parpadeo
            mensajeAvisoTexto.color = new Color(1f, 0.3f, 0.3f, alpha);
            yield return null;
        }

        mensajeAvisoTexto.gameObject.SetActive(false);
    }
}
