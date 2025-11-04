using UnityEngine;
using System.Collections.Generic;

public class JokerManager : MonoBehaviour
{
    public List<Joker> jokersInicioRonda = new List<Joker>();
    public List<Joker> jokersFinalRonda = new List<Joker>();
    public Transform zonaVisual; // un contenedor en la escena donde se verán los jokers activos
    public GameObject prefabIconoJoker; // un pequeño icono para mostrar los activos

    public void ActivarJokersInicio(GameManager gameManager)
    {
        foreach (var joker in jokersInicioRonda)
        {
            if (joker != null)
                joker.AplicarEfecto(gameManager);
        }
    }

    public void ActivarJokersFinal(GameManager gameManager)
    {
        foreach (var joker in jokersFinalRonda)
        {
            if (joker != null)
                joker.AplicarEfecto(gameManager);
        }
    }

    public void AddJokerInicio(Joker nuevo)
    {
        if (nuevo != null)
        {
            jokersInicioRonda.Add(nuevo);
            ActualizarVisual();
        }
    }

    public void AddJokerFinal(Joker nuevo)
    {
        if (nuevo != null)
        {
            jokersFinalRonda.Add(nuevo);
            ActualizarVisual();
        }
    }

    // ✅ Actualiza visualmente los jokers activos
    public void ActualizarVisual()
    {
        if (zonaVisual == null || prefabIconoJoker == null)
        {
            Debug.LogWarning("⚠️ Falta asignar zonaVisual o prefabIconoJoker en JokerManager.");
            return;
        }

        foreach (Transform hijo in zonaVisual)
            Destroy(hijo.gameObject);

        foreach (var joker in jokersInicioRonda)
        {
            if (joker != null)
            {
                var icono = Instantiate(prefabIconoJoker, zonaVisual);
                var img = icono.GetComponent<UnityEngine.UI.Image>();
                if (img != null && joker.icono != null)
                    img.sprite = joker.icono;
            }
        }
    }
}
