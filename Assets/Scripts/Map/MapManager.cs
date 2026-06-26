using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class MapManager : MonoBehaviour
{
    private IMapService _mapService;

    private IEventService _eventService;
    private IAudioService _audioService;

    public List<(MapTypess, int altura, int ancho)> map;

    [SerializeField] private Image imagePrefab;

    [SerializeField] private Button Rouleta;

    [SerializeField] private Transform posInicial;

    [SerializeField] private AudioClip backGroundMusic;
    private Vector2 pIposInicial;


    [SerializeField] private int longitud = 3;

    [SerializeField] private int ancho = 3;

    [SerializeField] private float EspaciadoAltura = 200;
    [SerializeField] private float EspaciadoAncho = 200;

    [SerializeField] private UILineRenderer lineRenderer;

    [SerializeField] private Image PlayerTile;

    
    private GameObject PlayerInstanciado;
    void Start()
    {
        _mapService = AppContainer.Get<IMapService>();
        _eventService = AppContainer.Get<IEventService>();
        _audioService = AppContainer.Get<IAudioService>();

        _eventService.Subscribe<RouletterMapTileSelectedEvent>(AdvanceTile);
        generateMap();
        pIposInicial = posInicial.position;
        if (backGroundMusic != null) _audioService.PlayLoopSound(backGroundMusic);
    }
    public void generateMap()
    {
        if (_mapService != null && posInicial.childCount == 0)
        {
            if (_mapService.ReadMap() == null || _mapService.ReadMap().Count ==0)
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

        for (int i = 2; i < (_mapService.returnLongitud() - 1); i++)
        {
            createRoulette(i);
        }

        for (int i = 1; i < _mapService.returnLongitud(); i++)
        {
            var origenes = returnListado(i);
            var destinos = returnListado(i + 1);
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
            }
            else if (destinos != null)
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


        PlayerInstanciado = Instantiate(PlayerTile, posInicial.transform.position, quaternion.identity).gameObject;
        PlayerInstanciado.transform.SetParent(posInicial);
        PlayerInstanciado.name = ($"Player_{_mapService.GetPosicionJugador().altura}_{_mapService.GetPosicionJugador().ancho}");
        PlayerInstanciado.transform.position = returnTile(_mapService.GetPosicionJugador().altura, _mapService.GetPosicionJugador().ancho).transform.position;


        posInicial.GetComponentInParent<DragMap>().CalcularLimites();
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
        tile.GetComponent<MapTile>().SetType(map[index].Item1);
    }


    private Sprite tileImage(MapTypess tipo)
    {

        return Resources.Load<Sprite>($"mapa/{tipo.ToString()}");

    }
    public void clearMap()
    {
        _mapService.ResetMap();
        clearChilds();
        StopAllCoroutines();
        posInicial.transform.position = pIposInicial;
        if (_mapService.GetMoving() == true) _mapService.ToggleMoving();
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

        if (ruleta.GetComponent<ruleTileButton>() != null)
        {
            ruleta.GetComponent<ruleTileButton>().AddListadoOpciones(returnListado(alto+1));
            ruleta.GetComponent<ruleTileButton>().setPosicion(alto);
        }


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

    private GameObject returnTile(int altura, int ancho)
    {
        foreach (Transform child in posInicial.transform)
        {
            if (child.name == ($"Tile_{altura}_{ancho}"))
            {
                return child.gameObject;
            }
        }
        return null;
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

    public void AdvanceTile()
    {
        int alturaPlayer = _mapService.GetPosicionJugador().altura;
        int anchoPlayer = _mapService.GetPosicionJugador().ancho;
        
        var destino = returnTile(alturaPlayer + 1, 1);
        var ruleta = returnRuleta(alturaPlayer);


        if (destino != null && ruleta == null && PlayerInstanciado != null && !_mapService.GetMoving() )
        {
            _mapService.ToggleMoving();
            StartCoroutine(MovePlayer(PlayerInstanciado.transform, destino, 2,true));
            _mapService.SetPositionPlayer(alturaPlayer + 1, 1);
            
        }
    }

    public void AdvanceTile(GameEventBase index)
    {
        int alturaPlayer = _mapService.GetPosicionJugador().altura;
        int anchoPlayer = _mapService.GetPosicionJugador().ancho;

        RouletterMapTileSelectedEvent indexado = (RouletterMapTileSelectedEvent)index;

        GameObject destino = null;

       foreach (Transform child in posInicial)
        {
            if (child.name == $"Tile_{alturaPlayer + 1}_{indexado.ordenHIjo + 1}")
            {
                destino = child.gameObject;
                
            }
        }

        var ruleta = returnRuleta(alturaPlayer);


        if (destino != null  && PlayerInstanciado != null && !_mapService.GetMoving() && ruleta != null)
        {
         
                _mapService.ToggleMoving();
            
            
           StartCoroutine(MovePlayerTroughRoulette(PlayerInstanciado.transform,ruleta, destino, 1));

            _mapService.SetPositionPlayer(alturaPlayer+1, indexado.ordenHIjo+1);

        }
    }


    IEnumerator MovePlayer(Transform Player,GameObject destino,float duracion, bool lastMovement)
    {
        Vector2 inicio = Player.position;

        float tiempo = 0;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;

            Player.position =
                Vector2.Lerp(
                    inicio,
                    destino.transform.position,
                    tiempo / duracion
                );

            yield return null;
        }

        Player.position = destino.transform.position;

        if (destino.TryGetComponent<MapTile>(out var tile))
        {
            tile.Execute();
        }
        pIposInicial = posInicial.position;
        if (lastMovement)
        {
            if (_mapService.GetMoving())
            {
                _mapService.ToggleMoving();
            }
        }
    }

    IEnumerator MovePlayerTroughRoulette(Transform Player,  GameObject ruleta, GameObject destino, float duracion)
    {
        if (ruleta != null)
        {
            yield return StartCoroutine(MovePlayer(Player, ruleta,duracion,false));
        }

        yield return StartCoroutine(MovePlayer(Player, destino, duracion,true));

        if (_mapService.GetMoving())
        {
            _mapService.ToggleMoving();
        }
    }


 

    public void cleanScreen()
    {
        clearChilds();
    }

    public void OnDestroy()
    {
        _eventService.Unsubscribe<RouletterMapTileSelectedEvent>(AdvanceTile);
        if (backGroundMusic != null) _audioService.StopSound(backGroundMusic);

    }
}







