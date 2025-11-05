using UnityEngine;

[CreateAssetMenu(fileName = "NuevoComodin", menuName = "Tienda/Comodin")]
public class Joker1 : ScriptableObject
{
    [Header("Datos base")]
    public string nombre;
    public Sprite icono;
    public int precioCompra;
    public int precioVenta;
    [TextArea] public string descripcion;

    public enum TipoEfecto { Ninguno, MasTiro, DoblePuntos, BolaFantasma, EscudoRebote, MenosMeta }
    [Header("Tipo de efecto")] public TipoEfecto tipoEfecto = TipoEfecto.Ninguno;

    /// <summary>
    /// Aplica el efecto al inicio de la nueva ronda.
    /// </summary>
    public void AplicarEfecto(GameManager gm)
    {
        if (!gm) { Debug.LogWarning("⚠️ [Joker1] GameManager nulo."); return; }

        switch (tipoEfecto)
        {
            case TipoEfecto.MasTiro:
                gm.AddTiros(1);
                Debug.Log($"🎯 [{nombre}] +1 Tiro aplicado al empezar la ronda.");
                break;

            case TipoEfecto.DoblePuntos:
                // Marca un flag en el GM si quieres duplicar puntos del próximo turno.
                // Por ahora lo dejamos como log para no romper nada.
                Debug.Log($"💥 [{nombre}] Doble Puntos (implementa el multiplicador en el cálculo de puntos).");
                break;

            case TipoEfecto.BolaFantasma:
                Debug.Log($"👻 [{nombre}] Bola Fantasma (activa un flag en tus colisiones).");
                break;

            case TipoEfecto.EscudoRebote:
                Debug.Log($"🛡️ [{nombre}] Escudo de Rebote (activa un flag para perdonar un fallo).");
                break;

            default:
                Debug.Log($"ℹ️ [{nombre}] sin efecto.");
                break;
        }

        gm.uiManager?.ActualizarHUD();
    }
}
