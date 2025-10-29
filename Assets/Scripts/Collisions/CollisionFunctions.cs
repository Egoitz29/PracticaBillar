using System;
using System.Diagnostics.Contracts;
using UnityEngine;

public static class CollisionFunctions
{
    public static bool CircleToCircle1(Vector2 posA, float radiusA, Vector2 posB, float radiusB,
                                        out Vector2 contactDirection, out float contactMagnitude, out Vector2 contactPoint)
    {
        // Calculamos la distancia entre los centros de los círculos
        Vector2 distanceVector = posB - posA;
        float distanceSquared = distanceVector.sqrMagnitude;

        // Sumamos los radios de los círculos
        float radiusSum = radiusA + radiusB;

        // Si los círculos colisionan
        if (distanceSquared <= radiusSum * radiusSum)
        {
            // Dirección de contacto: normalizamos el vector de distancia entre los círculos
            contactDirection = distanceVector.normalized;

            // Magnitud del contacto: la suma de los radios menos la distancia entre los círculos
            contactMagnitude = radiusSum - Mathf.Sqrt(distanceSquared);

            // Punto de contacto: el centro de uno de los círculos más la dirección de contacto multiplicada por el radio
            contactPoint = posA + contactDirection * radiusA;

            return true;
        }
        else
        {
            // Si no hay colisión, devolvemos valores predeterminados
            contactDirection = Vector2.zero;
            contactMagnitude = 0f;
            contactPoint = Vector2.zero;

            return false;
        }
    }
    public static bool CircleToAABBResolution(Vector2 centerCircle, float radiusCircle, Vector2 centerAABB, Vector2 halfSizeAABB, out Vector2 contactDirection, out float contactMagnitude, out Vector2 contactPoint)
    {
        contactPoint = Vector2.zero;
        contactDirection = Vector2.zero;
        contactMagnitude = 0f;

        // Calcular el punto más cercano en el AABB al centro del círculo
        float closestX = Mathf.Clamp(centerCircle.x, centerAABB.x - halfSizeAABB.x, centerAABB.x + halfSizeAABB.x);
        float closestY = Mathf.Clamp(centerCircle.y, centerAABB.y - halfSizeAABB.y, centerAABB.y + halfSizeAABB.y);
        contactPoint = new Vector2(closestX, closestY);

        // Calcular la dirección y distancia
        Vector2 direction = centerCircle - contactPoint;
        float distance = direction.magnitude;

        // Si la distancia es menor que el radio del círculo, hay colisión
        if (distance < radiusCircle)
        {
            // Evitar división por cero
            if (distance == 0)
            {
                direction = new Vector2(1, 0); // Dirección arbitraria
            }
            else
            {
                direction.Normalize();
            }

            contactDirection = direction;
            contactMagnitude = radiusCircle - distance;
            return true;
        }
        return false;
    }




}