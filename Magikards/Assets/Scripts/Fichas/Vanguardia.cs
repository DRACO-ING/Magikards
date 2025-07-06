using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vanguardia : Ficha
{
    private void Start()
    {
        // Inicialización de la Vanguardia
        // Asignación de puntos de vida y ataque
        pv = 7;
        pa = 1;
    }

    // Movimientos posibles de la Vanguardia:
    public override List<Vector2Int> ObtenerMovimientosPosibles(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY){
        List<Vector2Int> r = new List<Vector2Int>();

        // Movimiento hacia arriba
        for (int i = 1, y = actualY + 1; i <= 4 && y < cantCasillasY; i++, y++) {
            if (tablero[actualX, y] == null) {
                r.Add(new Vector2Int(actualX, y));
            } else {
                break;
            }
        }
        // Movimiento hacia abajo
        for (int i = 1, y = actualY - 1; i <= 4 && y >= 0; i++, y--) {
            if (tablero[actualX, y] == null) {
                r.Add(new Vector2Int(actualX, y));
            } else {
                break;
            }
        }
        
        // Movimiento hacia la derecha
        for (int i = 1, x = actualX + 1; i <= 4 && x < cantCasillasX; i++, x++) {
            if (tablero[x, actualY] == null) {
                r.Add(new Vector2Int(x, actualY));
            } else {
                break;
            }
        }
        // Movimiento hacia la izquierda
        for (int i = 1, x = actualX - 1; i <= 4 && x >= 0; i++, x--) {
            if (tablero[x, actualY] == null) {
                r.Add(new Vector2Int(x, actualY));
            } else {
                break;
            }
        } 
        return r;
    }
}
