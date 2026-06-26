
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ruleTileButton : MonoBehaviour
{
    private List<GameObject> listadoOpciones;

    
    private int alto = 0;

    [SerializeField] private GameObject ruleta;
    [SerializeField] private GameObject StopButton;
    private bool used = false;

    private IMapService _mapService;
    private void Start()
    {
        _mapService = AppContainer.Get<IMapService>();
    }
    public void AddListadoOpciones(List<GameObject> tiles)
    {
        listadoOpciones = tiles;

        Debug.Log(listadoOpciones.ToString());
    }

    public void setPosicion(int altoX)
    {
        
        this.alto = altoX;
    }

    public void MakeRuleta()
    {
        if (_mapService.GetPosicionJugador().altura == alto && !_mapService.GetMoving() && !used)
        {
            _mapService.ToggleMoving();
            used = true;
            List<Ficha> fichas = new List<Ficha>();

            var rule = Instantiate(ruleta,new Vector2(-7,2),quaternion.identity);
            var stopBTN = Instantiate(StopButton, new Vector2(-7, 0), quaternion.identity);
            foreach (GameObject tile in listadoOpciones)
            {
                var mapTile = tile.GetComponent<MapTile>();
                if ( mapTile!= null)
                {
                    fichas.Add(getTypeTile(mapTile));
                }
            }
            
            rule.GetComponentInChildren<WheelMapManager>().Generate(fichas.Count, fichas);
            rule.transform.parent = transform;
            stopBTN.GetComponent<Button>().onClick.RemoveAllListeners();
            stopBTN.GetComponent<Button>().onClick.AddListener(rule.GetComponentInChildren<WheelController>().StopSpin);
            StartCoroutine(Spin(rule.GetComponentInChildren<WheelController>()));
        }
    }


    IEnumerator Spin(WheelController ruleta)
    {
        ruleta.StartSpin();
        yield return null;//new WaitForSeconds(UnityEngine.Random.Range(0.1f, 2f));
        //ruleta.StopSpin();
        
    }

    private Ficha getTypeTile(MapTile tile)
    {

        return AddFicha(Resources.Load<FichaData>($"mapa/{tile.getTileType().ToString()}_Ficha"));

    }

    private Ficha AddFicha(FichaData ficha)
    {
        Ficha fichanueva = new Ficha(ficha);
        return fichanueva;
    }
  


}
