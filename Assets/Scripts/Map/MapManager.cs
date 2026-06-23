using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.Mathematics;
using UnityEngine.UI.Extensions;

public class MapManager : MonoBehaviour
{
    private IMapService _mapService;

    public List<(MapTypess, int altura, int ancho)> map;

    [SerializeField] private Image imagePrefab;

    [SerializeField] private Button Rouleta;

    [SerializeField] private Transform posInicial;


    [SerializeField] private int longitud = 3;

    [SerializeField] private int ancho = 3;

    [SerializeField] private float EspaciadoAltura = 200;
    [SerializeField] private float EspaciadoAncho = 200;

    [SerializeField] private UILineRenderer lineRenderer;
    void Start()
    {
        _mapService = AppContainer.Get<IMapService>();

        generateMap();

    }

    public void generateMap()
    {
        if (_mapService != null)
        {
            if (_mapService.ReadMap() == null)
            {
                _mapService.generateMap(longitud, ancho);
            }
            if (_mapService.GetPosicionJugador() == (0, 0))
            {
                _mapService.SetPositionPlayer(0, 1);
            }


            generateDisplay();
        }
    }

    void generateDisplay()
    {
        clearChilds();
        map = _mapService.ReadMap();

        for (int i = 0; i < map.Count; i++)
        {

            createTile(i);

        }

        for (int i = 2; i < _mapService.returnLongitud(); i++)
        {
            createRoulette(i);
        }

        for (int i = 1; i < _mapService.returnLongitud(); i++)
        {
            var origenes = returnListado(i);
            var destinos = returnListado(i+1);
            var ruleta = returnRuleta(i);

            // SI hay ruleta → Tile → Ruleta → Tile siguiente
            if (ruleta != null)
            {
                foreach (var origen in origenes)
                {
                    CrearLineas(
                        origen.transform,
                        ruleta.transform
                    );
                }

                if (destinos != null)
                {
                    foreach (var destino in destinos)
                    {
                        CrearLineas(
                            ruleta.transform,
                            destino.transform
                        );
                    }
                }
            }else if (destinos != null)
            {
                // SI NO hay ruleta → Tile → Tile
                foreach (var origen in origenes)
                {
                    foreach (var destino in destinos)
                    {
                        CrearLineas(
                            origen.transform,
                            destino.transform
                        );
                    }
                }
            }
        }

    }



    private void createTile(int index)
    {
        Debug.Log(map[index].ToString());

        float espacio = 0;

        if (returnPosY(map[index].altura) == 0) { espacio = 0; } else { espacio = EspaciadoAltura / 2; }
        Vector3 posicion = new Vector3(
    posInicial.position.x - EspaciadoAncho * returnPosX(map[index].ancho),
    posInicial.position.y - EspaciadoAltura * returnPosY(map[index].altura) + espacio,
    posInicial.position.z
);
        var tile = Instantiate(imagePrefab, posicion, quaternion.identity);
        tile.transform.SetParent(posInicial);
        tile.name = ($"Tile_{map[index].altura}_{map[index].ancho}");
        tile.sprite = tileImage(map[index].Item1);
    }


    private Sprite tileImage(MapTypess tipo)
    {

        return Resources.Load<Sprite>($"mapa/{tipo.ToString()}");

    }
    public void clearMap()
    {
        _mapService.ResetMap();
        clearChilds();
    }

    private void clearChilds()
    {
        foreach (Transform child in posInicial)
        {
            Destroy(child.gameObject);
        }
    }

    private int returnPosX(int ancho)
    {
        if (ancho == 1)
        {
            return 0;
        }
        else

            if (ancho % 2 == 0)
            {
                return ancho / 2;
            }
            else
            {
                return -(ancho / 2);
            }

    }

    private int returnPosY(int alto)
    {
        if (alto == 1)
        {
            return 0;
        }
        else
        {
            return alto - 1;
        }
    }

    private void createRoulette(int alto)
    {


        Vector3 posicion = new Vector3(
    posInicial.position.x,
    posInicial.position.y - EspaciadoAltura * (alto - 1),
    posInicial.position.z
    );
        var ruleta = Instantiate(Rouleta, posicion, quaternion.identity);
        ruleta.transform.SetParent(posInicial);
        ruleta.name = ($"Ruleta_{alto}");




    }

    private GameObject returnRuleta(int altura)
    {
        foreach (Transform child in posInicial.transform)
        {
            for (int i = 0; i <= _mapService.returnAncho(); i++)
            {
                if (child.name == ($"Ruleta_{altura}"))
                {
                    return child.gameObject;
                }
            }
        }
        return null;
    }


    private List<GameObject> returnListado(int altura)
    {
        List<GameObject> listado = new List<GameObject>();
        foreach (Transform child in posInicial.transform)
        {
            for (int i = 0; i <= _mapService.returnAncho(); i++)
            {
                if (child.name == ($"Tile_{altura}_{i}"))
                {
                    listado.Add(child.gameObject);
                }
            }
        }

        return listado;
    }

    private void CrearLineas(Transform posInicialVertex, Transform posFinal)
    {
        Vector2[] vertex = new Vector2[2];
        vertex[0] = posInicialVertex.transform.position;
        vertex[1] = posFinal.transform.position;

        var linea = Instantiate(lineRenderer, Vector3.zero, quaternion.identity);
        linea.transform.SetParent(posInicial);
        linea.transform.SetAsFirstSibling();
        //linea.transform.SetParent(posInicialVertex);
        linea.Points = vertex;

    }







}

