using UnityEngine;
using System.Collections.Generic;

// Enum para definir los tipos de fichas
public enum TipoFicha{
    Archimago = 0,
    Augur = 1,
    Arcanista = 2,
    Vanguardia = 3
}
public class Ficha : MonoBehaviour{
    // Propiedades de la ficha
    public int equipo;
    public int actualX;
    public int actualY;
    public TipoFicha tipoFicha;
    protected int pv;
    protected int pa;

    private Vector3 posicionDeseada;
    private Vector3 escalaDeseada;

    // Inicialización de la ficha
    protected virtual void Awake(){
        // Inicializar la escala deseada con la escala actual del transform para evitar cambios bruscos
        escalaDeseada = transform.localScale;
    }

    private void Update(){
        // Actualizar la posición y escala de la ficha suavemente
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, Time.deltaTime * 5f);
        transform.localScale = Vector3.Lerp(transform.localScale, escalaDeseada, Time.deltaTime * 5f);
    }

    // Método para obtener los movimientos posibles de la ficha
    public virtual List<Vector2Int> ObtenerMovimientosPosibles(ref Ficha[,] tablero, int CANT_CASILLAS_X, int CANT_CASILLAS_Y)
    {
        // Este método debe ser implementado por las subclases
        List<Vector2Int> movimientosPosibles = new List<Vector2Int>();

        // Ejemplo de movimientos posibles para una ficha genérica
        movimientosPosibles.Add(new Vector2Int(5, 5));
        movimientosPosibles.Add(new Vector2Int(5, 6));
        movimientosPosibles.Add(new Vector2Int(5, 7));
        movimientosPosibles.Add(new Vector2Int(6, 5));
        movimientosPosibles.Add(new Vector2Int(6, 6));
        movimientosPosibles.Add(new Vector2Int(6, 7));
        movimientosPosibles.Add(new Vector2Int(7, 5));
        movimientosPosibles.Add(new Vector2Int(7, 6));
        movimientosPosibles.Add(new Vector2Int(7, 7));

        return movimientosPosibles;
    }

    // Métodos para definir la posición y escala deseada de la ficha
    public virtual void DefinirPosicion(Vector3 posicion, bool forzar = false)
    {
        posicionDeseada = posicion;
        if (forzar)
        {
            transform.position = posicionDeseada;
        }
    }
    public virtual void DefinirEscala(Vector3 escala, bool forzar = false)
    {
        escalaDeseada = escala;
        if (forzar)
        {
            transform.localScale = escalaDeseada;
        }
    }
}
