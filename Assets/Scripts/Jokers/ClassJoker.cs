using UnityEngine;

public abstract class Joker : ScriptableObject
{
    public string nombre;
    public string descripcion;
    public Sprite icono;

    public abstract void AplicarEfecto(GameManager gameManager);
   
}
