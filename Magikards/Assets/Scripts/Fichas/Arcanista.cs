using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Arcanista : Ficha{
    private void Start() {
        pv = 5;
        pa = 2;
    }

    //Movimientos posibles del Arcanista:
    public override List<Vector2Int> ObtenerMovimientosPosibles(ref Ficha[,] tablero, int cantCasillasX, int cantCasillasY) {
        List<Vector2Int> r = new List<Vector2Int>();

        // Movimiento diagonal superior derecha
        for (int i = 1, x = actualX + 1, y = actualY + 1; i <= 4 && x < cantCasillasX && y < cantCasillasY; i++, x++, y++) {
            if (tablero[x, y] == null) {
                r.Add(new Vector2Int(x, y));
            } else {
                break;
            }
        }

        // Movimiento diagonal superior izquierda
        for (int i = 1, x = actualX - 1, y = actualY + 1; i <= 4 && x >= 0 && y < cantCasillasY; i++, x--, y++) {
            if (tablero[x, y] == null) {
                r.Add(new Vector2Int(x, y));
            } else {
                break;
            }
        }

        // Movimiento diagonal inferior derecha
        for (int i = 1, x = actualX + 1, y = actualY - 1; i <= 4 && x < cantCasillasX && y >= 0; i++, x++, y--) {
            if (tablero[x, y] == null) {
                r.Add(new Vector2Int(x, y));
            } else {
                break;
            }
        }

        // Movimiento diagonal inferior izquierda
        for (int i = 1, x = actualX - 1, y = actualY - 1; i <= 4 && x >= 0 && y >= 0; i++, x--, y--) {
            if (tablero[x, y] == null) {
                r.Add(new Vector2Int(x, y));
            } else {
                break;
            }
        }

        return r;
    }
}
