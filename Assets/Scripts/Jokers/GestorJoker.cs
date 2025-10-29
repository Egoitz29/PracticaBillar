
using UnityEngine;
using System.Collections.Generic;

public class JokerManager : MonoBehaviour
{
    public List<Joker> jokersInicioRonda = new List<Joker>();
    public List<Joker> jokersFinalRonda = new List<Joker>();


    public void ActivarJokersInicio(GameManager gameManager)
    {
        foreach (var joker in jokersInicioRonda)
        {
            joker.AplicarEfecto(gameManager);
        }
    }

    public void ActivarJokersFinal(GameManager gameManager)
    {
        for (int i = 0; i < jokersFinalRonda.Count; i++)
        {
            var joker = jokersFinalRonda[i];
            if (joker == null)
            {
                Debug.LogWarning($"[JokerManager] Joker nulo en la lista de final de ronda en índice {i}. Revisa la escena.");
                continue;
            }

            joker.AplicarEfecto(gameManager);
        }
    }


    public void AddJokerInicio(Joker nuevo)
    {
        jokersInicioRonda.Add(nuevo);
        // Podrías lanzar un evento o actualizar UI aquí
    }

    public void AddJokerFinal(Joker nuevo)
    {
        if (nuevo == null)
        {
            Debug.LogWarning("Se intentó añadir un Joker FINAL nulo.");
            return;
        }
        jokersFinalRonda.Add(nuevo);
    }

}
