using System.Collections.Generic;
using UnityEngine;

public class Banda : MonoBehaviour
{
    [SerializeField] private List<Comodin> comodines = new List<Comodin>();
    [SerializeField] private Transform[] posicionesComodines; // Posiciones físicas donde colocar los comodines

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("La bola ha colisionado");
        if (collision.gameObject.CompareTag("BolaBlanca"))
        {
            Debug.Log("Ha pillado el tag" );
            AplicarComodines(collision.gameObject);
        }
    }

    public void AplicarComodines(GameObject bola)
    {
        int multiplicador = 1;

        // Primero evaluamos si hay multiplicador
        foreach (Comodin comodin in comodines)
        {
            if (comodin is CMultiplicador)
            {
                multiplicador = 2;
                break;
            }
        }

        foreach (Comodin comodin in comodines)
        {
            for (int i = 0; i < multiplicador; i++)
            {
                if (!(comodin is CMultiplicador)) // Evita aplicar el mismo comodín multiplicador más de una vez
                {
                    comodin.Aplicar(bola);
                }
            }
        }
    }
    public void SumarPuntos(ContadorRebotesAntesDeBlanca contador)
    {
        contador.ContarRebote();
        Debug.Log("Banda Normal: Rebote contado. Total rebotes: " + contador.rebotes);
    }

    public bool AñadirComodin(Comodin nuevoComodin)
    {
        if (comodines.Count >= 3)
        {
            Debug.Log("La banda ya tiene 3 comodines");
            return false;
        }

        comodines.Add(nuevoComodin);
        ActualizarPosicionesVisuales();
        return true;
    }

    public void QuitarComodin(Comodin comodin)
    {
        comodines.Remove(comodin);
        Destroy(comodin.gameObject);
        ActualizarPosicionesVisuales();
    }

    private void ActualizarPosicionesVisuales()
    {
        for (int i = 0; i < comodines.Count; i++)
        {
            comodines[i].transform.position = posicionesComodines[i].position;
            // Opcional: cambiar color según tipo
            SpriteRenderer sr = comodines[i].GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.color = comodines[i].ObtenerColor();
        }
    }
}
