using System;
using UnityEngine;

public class Tablero : MonoBehaviour
{
    private const int CANT_CASILLAS_X = 13;
    private const int CANT_CASILLAS_Y = 16;
    private GameObject[,] casillas;
    private Camera camaraActual;
    private Vector2Int casillaCursor;
    private Vector3 bordes;

    [Header("Configuración del Tablero")]
    [SerializeField] private float tamanoCasillas = 1.0f;
    [SerializeField] private float desplazamientoY = 0.2f;
    [SerializeField] private Vector3 centroTablero = Vector3.zero;

    [Header("Arte del Tablero")]
    [SerializeField] private Material materialCasilla;

    private void Awake()
    {
        GenerarTodasLasCasillas(tamanoCasillas, CANT_CASILLAS_X, CANT_CASILLAS_Y);
    }

    private void Update()
    {
        if (!camaraActual)
        {
            camaraActual = Camera.main;
            return;
        }

        RaycastHit info;
        Ray rayo = camaraActual.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(rayo, out info, 100, LayerMask.GetMask("Casilla", "Cursor")))
        {
            Vector2Int posicionToque = VerificarIndiceCasilla(info.transform.gameObject);

            if (casillaCursor == -Vector2Int.one)
            {
                casillaCursor = posicionToque;
                casillas[posicionToque.x, posicionToque.y].layer = LayerMask.NameToLayer("Cursor");
            }

            if (casillaCursor != posicionToque)
            {
                casillas[casillaCursor.x, casillaCursor.y].layer = LayerMask.NameToLayer("Casilla");
                casillaCursor = posicionToque;
                casillas[posicionToque.x, posicionToque.y].layer = LayerMask.NameToLayer("Cursor");
            }
        }
        else
        {
            if (casillaCursor != -Vector2Int.one)
            {
               casillas[casillaCursor.x, casillaCursor.y].layer = LayerMask.NameToLayer("Casilla");
                casillaCursor = -Vector2Int.one; // Resetea la casilla del cursor
            }
        }
    }

    private void GenerarTodasLasCasillas(float tamanoCasillas, int cantCasillasX, int cantCasillasY){
        desplazamientoY += transform.position.y;
        bordes = new Vector3((cantCasillasX / 2) * tamanoCasillas + (tamanoCasillas/2), 0, (cantCasillasY / 2) * tamanoCasillas) + centroTablero;


        casillas = new GameObject[cantCasillasX, cantCasillasY];

        for (int x = 0; x < cantCasillasX; x++)
        {
            for (int y = 0; y < cantCasillasY; y++)
            {
                casillas[x, y] = GenerarUnaCasilla(tamanoCasillas, x, y);
            }
        }
    }

    private GameObject GenerarUnaCasilla(float tamanoCasilla, int x, int y){
        GameObject objetoCasilla = new GameObject($"Casilla_{x + 1}_{y + 1}");
        objetoCasilla.transform.parent = transform;

        Mesh malla = new Mesh();
        objetoCasilla.AddComponent<MeshFilter>().mesh = malla;
        objetoCasilla.AddComponent<MeshRenderer>().material = materialCasilla;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tamanoCasilla, desplazamientoY, y * tamanoCasilla) - bordes;
        vertices[1] = new Vector3(x * tamanoCasilla, desplazamientoY, (y + 1) * tamanoCasilla) - bordes;
        vertices[2] = new Vector3((x + 1) * tamanoCasilla, desplazamientoY, y * tamanoCasilla) - bordes;
        vertices[3] = new Vector3((x + 1) * tamanoCasilla, desplazamientoY, (y + 1) * tamanoCasilla) - bordes;

        int[] triangulos = new int[] { 0, 1, 2, 1, 3, 2 };

        malla.vertices = vertices;
        malla.triangles = triangulos;
        malla.RecalculateNormals();

        objetoCasilla.layer = LayerMask.NameToLayer("Casilla");
        objetoCasilla.AddComponent<BoxCollider>();

        return objetoCasilla;
    }

    private Vector2Int VerificarIndiceCasilla(GameObject infoToque){
        for (int x = 0; x < CANT_CASILLAS_X; x++){
            for (int y = 0; y < CANT_CASILLAS_Y; y++){
                if (casillas[x, y] == infoToque){
                    return new Vector2Int(x, y);
                }
            }
        }
        
        return -Vector2Int.one; // Retorna -1, -1 si no se encuentra la casilla

    }
}