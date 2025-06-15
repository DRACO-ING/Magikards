using System;
using UnityEngine;
using System.Collections.Generic;

public class Tablero : MonoBehaviour
{
    // Constantes del tablero
    private const int CANT_CASILLAS_X = 13;
    private const int CANT_CASILLAS_Y = 16;
    private GameObject[,] casillas;
    private Camera camaraActual;
    private Vector2Int casillaCursor;
    private Vector3 bordes;
    private Ficha[,] fichas;
    private Ficha fichaSeleccionada;
    private List<Vector2Int> movimientosPosibles = new List<Vector2Int>();

    // Configuración del tablero
    [Header("Configuración del Tablero")]
    [SerializeField] private float tamanoCasillas = 1.0f;
    [SerializeField] private float desplazamientoY = 0.2f;
    [SerializeField] private Vector3 centroTablero = Vector3.zero;

    // Arte del tablero
    [Header("Arte del Tablero")]
    [SerializeField] private Material materialCasilla;
    [SerializeField] private GameObject[] prefabsFichas;
    [SerializeField] private Material[] materialesEquipos;

    // Inicialización
    private void Awake(){
        GenerarTodasLasCasillas(tamanoCasillas, CANT_CASILLAS_X, CANT_CASILLAS_Y);
        GenerarTodasLasFichas();
        PosicionarTodasLasFichas();
    }
    // Actualización del tablero
    private void Update()
    {
        // Si no hay cámara asignada, intento obtener la cámara principal
        if (!camaraActual)
        {
            camaraActual = Camera.main;
            return;
        }

        // Raycast para detectar casillas
        RaycastHit info;
        Ray rayo = camaraActual.ScreenPointToRay(Input.mousePosition);
        bool hit = Physics.Raycast(rayo, out info, 100, LayerMask.GetMask("Casilla", "Cursor", "MovimientoPosible"));

        // Si se ha detectado una casilla
        if (hit)
        {
            // Verifico el índice de la casilla tocada
            Vector2Int toque = VerificarIndiceCasilla(info.transform.gameObject);

            // Primera vez que entra en una casilla
            if (casillaCursor == -Vector2Int.one)
            {
                // Si no hay ficha seleccionada, marco la casilla tocada como cursor
                casillaCursor = toque;
                casillas[toque.x, toque.y].layer = LayerMask.NameToLayer("Cursor");
            }
            // Cambia de casilla
            else if (casillaCursor != toque)
            {
                // Restauro el layer adecuado de la casilla anterior
                if (TieneMovimientoPosible(ref movimientosPosibles, casillaCursor))
                    casillas[casillaCursor.x, casillaCursor.y].layer = LayerMask.NameToLayer("MovimientoPosible");
                else
                    casillas[casillaCursor.x, casillaCursor.y].layer = LayerMask.NameToLayer("Casilla");

                // Marco la nueva como “Cursor”
                casillaCursor = toque;
                casillas[toque.x, toque.y].layer = LayerMask.NameToLayer("Cursor");
            }

            // Click izquierdo: selección o movimiento
            if (Input.GetMouseButtonDown(0))
            {
                // Si hay una ficha seleccionada, la deselecciono
                if (fichaSeleccionada == null)
                {
                    // Intento seleccionar ficha
                    if (fichas[toque.x, toque.y] != null /* && es tu turno */)
                        fichaSeleccionada = fichas[toque.x, toque.y];
                }
                else
                {
                    // Intento mover la ficha seleccionada
                    Vector2Int prev = new Vector2Int(fichaSeleccionada.actualX, fichaSeleccionada.actualY);
                    // Si la casilla tocada es la misma que la ficha seleccionada, la deselecciono
                    bool ok = MoverA(fichaSeleccionada, toque.x, toque.y);
                    // Si el movimiento es válido, actualizo la posición de la ficha
                    if (!ok)
                    {
                        // La devuelvo a su sitio si es inválido
                        fichaSeleccionada.DefinirPosicion(ObtenerCentroCasilla(prev.x, prev.y));
                        fichaSeleccionada = null;
                    }
                }
            }
        }
        else
        {
            // Al salir del raycast, restauro la casilla previa y limpio cursor
            if (casillaCursor != -Vector2Int.one)
            {
                // Si hay una ficha seleccionada, la deselecciono
                if (TieneMovimientoPosible(ref movimientosPosibles, casillaCursor))
                    casillas[casillaCursor.x, casillaCursor.y].layer = LayerMask.NameToLayer("MovimientoPosible");
                else
                    casillas[casillaCursor.x, casillaCursor.y].layer = LayerMask.NameToLayer("Casilla");

                // Limpio el cursor
                casillaCursor = -Vector2Int.one;
            }
        }

        // Ilumino o apago posibles movimientos
        if (fichaSeleccionada != null)
        {
            // Si hay una ficha seleccionada, obtengo sus movimientos posibles
            movimientosPosibles = fichaSeleccionada.ObtenerMovimientosPosibles(ref fichas, CANT_CASILLAS_X, CANT_CASILLAS_Y);
            // Ilumino las casillas posibles
            IluminarCasillas();
        }
        else
        {
            // Si no hay ficha seleccionada, desilumino las casillas
            DesiluminarCasillas();
        }
    }
    //Generación del tablero
    private void GenerarTodasLasCasillas(float tamanoCasillas, int cantCasillasX, int cantCasillasY){
        // Inicialización de variables
        desplazamientoY += transform.position.y;
        bordes = new Vector3((cantCasillasX / 2) * tamanoCasillas + (tamanoCasillas / 2), 0, (cantCasillasY / 2) * tamanoCasillas) + centroTablero;

        // Creación del array de casillas
        casillas = new GameObject[cantCasillasX, cantCasillasY];

        // Generación de las casillas
        for (int x = 0; x < cantCasillasX; x++)
        {
            for (int y = 0; y < cantCasillasY; y++)
            {
                casillas[x, y] = GenerarUnaCasilla(tamanoCasillas, x, y);
            }
        }
    }
    // Generación de una casilla
    private GameObject GenerarUnaCasilla(float tamanoCasilla, int x, int y){
        // Creación del GameObject de la casilla
        GameObject objetoCasilla = new GameObject($"Casilla_{x + 1}_{y + 1}");
        // Asignación de la posición y tamaño
        objetoCasilla.transform.parent = transform;

        // Ajuste de la malla de la casilla
        Mesh malla = new Mesh();
        objetoCasilla.AddComponent<MeshFilter>().mesh = malla;
        objetoCasilla.AddComponent<MeshRenderer>().material = materialCasilla;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tamanoCasilla, desplazamientoY, y * tamanoCasilla) - bordes;
        vertices[1] = new Vector3(x * tamanoCasilla, desplazamientoY, (y + 1) * tamanoCasilla) - bordes;
        vertices[2] = new Vector3((x + 1) * tamanoCasilla, desplazamientoY, y * tamanoCasilla) - bordes;
        vertices[3] = new Vector3((x + 1) * tamanoCasilla, desplazamientoY, (y + 1) * tamanoCasilla) - bordes;

        int[] triangulos = new int[] { 0, 1, 2, 1, 3, 2 };

        // Asignación de los vértices y triángulos a la malla
        malla.vertices = vertices;
        malla.triangles = triangulos;
        malla.RecalculateNormals();

        // Asignación de la capa y colisionador
        objetoCasilla.layer = LayerMask.NameToLayer("Casilla");
        objetoCasilla.AddComponent<BoxCollider>();

        // Retorno del objeto casilla
        return objetoCasilla;
    }

    //Generación de las fichas
    private void GenerarTodasLasFichas(){
        // Inicialización del array de fichas
        fichas = new Ficha[CANT_CASILLAS_X, CANT_CASILLAS_Y];
        // Definición de los equipos
        int equipo1 = 0, equipo2 = 1;

        // Generación de las fichas en posiciones específicas para cada equipo
        //Equipo 1
        fichas[4, 0] = GenerarUnaFicha(TipoFicha.Augur, equipo1);
        fichas[6, 1] = GenerarUnaFicha(TipoFicha.Archimago, equipo1);
        fichas[8, 0] = GenerarUnaFicha(TipoFicha.Augur, equipo1);
        fichas[5, 2] = GenerarUnaFicha(TipoFicha.Arcanista, equipo1);
        fichas[7, 2] = GenerarUnaFicha(TipoFicha.Arcanista, equipo1);
        fichas[4, 3] = GenerarUnaFicha(TipoFicha.Vanguardia, equipo1);
        fichas[6, 3] = GenerarUnaFicha(TipoFicha.Vanguardia, equipo1);
        fichas[8, 3] = GenerarUnaFicha(TipoFicha.Vanguardia, equipo1);
        //Equipo 2
        fichas[4, 15] = GenerarUnaFicha(TipoFicha.Augur, equipo2);
        fichas[6, 14] = GenerarUnaFicha(TipoFicha.Archimago, equipo2);
        fichas[8, 15] = GenerarUnaFicha(TipoFicha.Augur, equipo2);
        fichas[5, 13] = GenerarUnaFicha(TipoFicha.Arcanista, equipo2);
        fichas[7, 13] = GenerarUnaFicha(TipoFicha.Arcanista, equipo2);
        fichas[4, 12] = GenerarUnaFicha(TipoFicha.Vanguardia, equipo2);
        fichas[6, 12] = GenerarUnaFicha(TipoFicha.Vanguardia, equipo2);
        fichas[8, 12] = GenerarUnaFicha(TipoFicha.Vanguardia, equipo2);
    }
    private Ficha GenerarUnaFicha(TipoFicha tipoFicha, int equipo){
        // Verifica que el tipo de ficha sea válido
        Ficha f = Instantiate(prefabsFichas[(int)tipoFicha], transform).GetComponent<Ficha>();

        // Asigna tipo de ficha, equipo y materiales
        f.tipoFicha = tipoFicha;
        f.equipo = equipo;
        Transform sombrero = f.transform.Find("Sombrero"); // Asegúrate que el nombre coincida

        // Verifica que el sombrero exista
        if (sombrero != null)
        {
            //Busca el MeshRenderer del sombrero
            MeshRenderer renderer = sombrero.GetComponent<MeshRenderer>();
            // Si tiene MeshRenderer, asigna el material correspondiente al equipo
            if (renderer != null)
            {
                renderer.material = materialesEquipos[equipo];
            }
            else
            {
                // Si no tiene MeshRenderer, muestra un mensaje de advertencia
                Debug.LogWarning("El sombrero no tiene un MeshRenderer.");
            }
        }
        else
        {
            // Si no se encuentra el sombrero, muestra un mensaje de advertencia
            Debug.LogWarning("No se encontró el objeto 'Sombrero' como hijo de la ficha.");
        }
        // Retorna la ficha generada
        return f;
    }

    //Posicionamiento de las fichas
    private void PosicionarTodasLasFichas(){
        // Recorre todas las casillas y posiciona las fichas
        for (int x = 0; x < CANT_CASILLAS_X; x++)
        {
            for (int y = 0; y < CANT_CASILLAS_Y; y++)
            {
                if (fichas[x, y] != null)
                {
                    PosicionarUnaFicha(x, y, true);
                }
            }
        }
    }
    // Posiciona una ficha en una casilla específica
    private void PosicionarUnaFicha(int x, int y, bool forzar = false){
        // Posiciona la ficha
        fichas[x, y].actualX = x;
        fichas[x, y].actualY = y;
        fichas[x, y].DefinirPosicion(ObtenerCentroCasilla(x, y), forzar);

        //Si forzar es verdadero, ajusta la rotación de la ficha
        if (forzar)
        {
            if (fichas[x, y].equipo == 0)
            {
                fichas[x, y].transform.localRotation = Quaternion.Euler(0f, -90f, -90f);
            }
            else if (fichas[x, y].equipo == 1)
            {
                fichas[x, y].transform.localRotation = Quaternion.Euler(180f, -90f, -90f);
            }
        }
    }

    //Operaciones
    // Obtiene el centro de una casilla específica
    private Vector3 ObtenerCentroCasilla(int x, int y)
    {
        // Verifica que los índices estén dentro del rango
        return new Vector3(x * tamanoCasillas, desplazamientoY, y * tamanoCasillas) - bordes + new Vector3(tamanoCasillas / 2, 0, tamanoCasillas / 2);
    }
    private Vector2Int VerificarIndiceCasilla(GameObject infoToque){
        // Recorre todas las casillas para encontrar la que coincide con la información del toque
        for (int x = 0; x < CANT_CASILLAS_X; x++)
        {
            for (int y = 0; y < CANT_CASILLAS_Y; y++)
            {
                if (casillas[x, y] == infoToque)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        return -Vector2Int.one; // Retorna -1, -1 si no se encuentra la casilla

    }
    private bool MoverA(Ficha ficha, int x, int y){
        // Verifica si hay un movimiento posible en la casilla destino
        if (!TieneMovimientoPosible(ref movimientosPosibles, new Vector2Int(x, y)))
        {
            // Si no hay un movimiento posible, muestra un mensaje de error y retorna false
            Debug.Log($"Movimiento a la casilla ({x}, {y}) no permitido.");
            return false;
        }

        // Guarda la posición anterior de la ficha
        Vector2Int posicionAnterior = new Vector2Int(ficha.actualX, ficha.actualY);

        // Verifica si la casilla destino está ocupada
        if (fichas[x, y] != null)
        {
            // Si la casilla está ocupada, muestra un mensaje de error y retorna false
            Ficha otraFicha = fichas[x, y];
            Debug.Log($"No se puede mover a la casilla ({x}, {y}) porque ya está ocupada por {otraFicha.tipoFicha} del equipo {otraFicha.equipo + 1}.");
            return false; // No se puede mover a una casilla ocupada
        }

        // Mueve la ficha a la nueva posición
        fichas[x, y] = ficha;
        // Actualiza para que la casilla anterior quede vacía
        fichas[posicionAnterior.x, posicionAnterior.y] = null;

        // Mueve la ficha a la nueva posición
        PosicionarUnaFicha(x, y);

        fichaSeleccionada = null; // Resetea la ficha seleccionada después de moverla

        //Retorna true si el movimiento fue exitoso
        return true;
    }
     private bool TieneMovimientoPosible(ref List<Vector2Int> movimientos, Vector2Int pos){
        // Verifica si la posición está dentro de los movimientos posibles
        for (int i = 0; i < movimientos.Count; i++)
        {
            if (movimientos[i] == pos)
                return true;
        }
        // Si no se encuentra la posición en los movimientos posibles, retorna false
        return false;
    }
    private void IluminarCasillas(){
        //Recorre la lista de movimientos posibles y cambia el layer de las casillas correspondientes
        for (int i = 0; i < movimientosPosibles.Count; i++)
        {
            // Verifica si la casilla no tiene el cursor encima
            if (casillas[movimientosPosibles[i].x, movimientosPosibles[i].y].layer == LayerMask.NameToLayer("Casilla"))
                // Cambia el layer de la casilla a "MovimientoPosible"
                casillas[movimientosPosibles[i].x, movimientosPosibles[i].y].layer = LayerMask.NameToLayer("MovimientoPosible");
        }
    }
    private void DesiluminarCasillas(){
        // Recorre la lista de movimientos posibles y restaura el layer de las casillas
        for (int i = 0; i < movimientosPosibles.Count; i++)
        {
            casillas[movimientosPosibles[i].x, movimientosPosibles[i].y].layer = LayerMask.NameToLayer("Casilla");
        }
        // Limpia la lista de movimientos posibles
        movimientosPosibles.Clear();
    }
}