using UnityEngine;
using System.Collections;



[CreateAssetMenu(fileName = "NuevoComodin", menuName = "Tienda/Comodin")]
public class Joker1 : ScriptableObject
{
    [Header("Datos base")]
    public string nombre;
    public Sprite icono;
    public int precioCompra;
    public int precioVenta;
    [TextArea] public string descripcion;

    public enum TipoEfecto
    {
        Ninguno,
        MasTiro,
        DoblePuntos,
        BolaFantasma,
        EscudoRebote,
        MenosMeta,
        AumentarEscala,
        DobleRebote// 🔹 Nuevo tipo de efecto
    }

    [Header("Tipo de efecto")]
    public TipoEfecto tipoEfecto = TipoEfecto.Ninguno;

    /// <summary>
    /// Aplica el efecto al inicio de la nueva ronda.
    /// </summary>
    public void AplicarEfecto(GameManager gm)
    {
        if (!gm)
        {
            Debug.LogWarning("⚠️ [Joker1] GameManager nulo.");
            return;
        }

        switch (tipoEfecto)
        {
            case TipoEfecto.MasTiro:
                gm.AddTiros(1);
                Debug.Log($"🎯 [{nombre}] +1 Tiro aplicado al empezar la ronda.");
                break;

            case TipoEfecto.DoblePuntos:
                Debug.Log($"💥 [{nombre}] Doble Puntos (implementa el multiplicador en el cálculo de puntos).");
                break;

            case TipoEfecto.BolaFantasma:
                Debug.Log($"👻 [{nombre}] Bola Fantasma activada.");
                break;

            case TipoEfecto.EscudoRebote:
                Debug.Log($"🛡️ [{nombre}] Escudo de Rebote activado.");
                break;

            case TipoEfecto.MenosMeta:
                gm.puntosRequeridos = Mathf.Max(1, gm.puntosRequeridos - 1);
                Debug.Log($"🎯 [{nombre}] Meta reducida a {gm.puntosRequeridos}.");
                gm.uiManager?.ActualizarHUD();
                break;

            case TipoEfecto.AumentarEscala:
                AumentarEscalaObjeto(gm);
                break;

            case TipoEfecto.DobleRebote:
                gm.dobleReboteActivo = true;
                Debug.Log($"💥 [{nombre}] Doble Rebote ACTIVADO → los rebotes valen x2");
                gm.uiManager?.MostrarAviso("💥 Rebotes x2 ACTIVADOS");
                break;


            default:
                Debug.Log($"ℹ️ [{nombre}] sin efecto.");
                break;
        }

        gm.uiManager?.ActualizarHUD();
    }

    // ============================================================
    // ⚙️ NUEVO MÉTODO → Aumentar Escala de un objeto concreto
    // ============================================================
    private void AumentarEscalaObjeto(GameManager gm)
    {
        if (gm.objetoEscalable == null)
        {
            Debug.LogWarning("⚠️ [Joker1] No se asignó 'objetoEscalable' en el GameManager.");
            return;
        }

        Transform t = gm.objetoEscalable.transform;
        Vector3 objetivo = new Vector3(1f, 1f, 1f);
        gm.StartCoroutine(EscalaSuave(t, objetivo, 0.3f));
        Debug.Log($"🟢 [{nombre}] Escala aumentada del objeto '{gm.objetoEscalable.name}'");
    }

    private IEnumerator EscalaSuave(Transform t, Vector3 objetivo, float duracion)
    {
        Vector3 inicio = t.localScale;
        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            t.localScale = Vector3.Lerp(inicio, objetivo, tiempo / duracion);
            yield return null;
        }

        t.localScale = objetivo;
    }


    /// <summary>
    /// Revertir efecto (por ejemplo, al vender el comodín)
    /// </summary>
    public void QuitarEfecto(GameManager gm)
    {
        if (!gm || gm.objetoEscalable == null) return;

        if (tipoEfecto == TipoEfecto.AumentarEscala)
        {
            Transform t = gm.objetoEscalable.transform;
            gm.StartCoroutine(EscalaSuave(t, new Vector3(0.75f, 0.75f, 0.75f), 0.3f));
            Debug.Log($"🔻 [{nombre}] Escala restaurada a (0.75, 0.75, 0.75)");
        }

        if (tipoEfecto == TipoEfecto.DobleRebote)
        {
            gm.dobleReboteActivo = false;
            Debug.Log($"⚪ [{nombre}] Doble Rebote DESACTIVADO → los rebotes vuelven a 1");
            gm.uiManager?.MostrarAviso("⚪ Rebotes x2 DESACTIVADOS");
        }


    }



}
