using System.Collections.Generic;
using UnityEngine;

public class RewardSelecter : MonoBehaviour
{
    [SerializeField] List<FichaData> ListaDeRecompensasPosibles;
    List<FichaData> ListaDeRecompensas = new List<FichaData>();
    void Awake()
    {
        for (int i = 0 ; i < 3 ; i++)
        {
            ListaDeRecompensas.Add(ListaDeRecompensasPosibles[Random.Range(0,ListaDeRecompensasPosibles.Count)]);
        }
        
    }
    void Start()
    {
        RewardInitializer initializer = this.gameObject.GetComponent<RewardInitializer>();
        
        initializer.setListaDeFichas(ListaDeRecompensas);
    }
}
