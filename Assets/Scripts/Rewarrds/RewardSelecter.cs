using System.Collections.Generic;
using UnityEngine;

public class RewardSelecter : MonoBehaviour
{
    [SerializeField] FichaData[] ListaDeRecompensasPosibles;
    List<FichaData> ListaDeRecompensas = new List<FichaData>();
    
    void Start()
    {
        ListaDeRecompensasPosibles = Resources.LoadAll<FichaData>("Objects/Rewards");
        for (int i = 0; i < 3; i++)
        {
            ListaDeRecompensas.Add(ListaDeRecompensasPosibles[Random.Range(0, ListaDeRecompensasPosibles.Length)]);
        }
        RewardInitializer initializer = this.gameObject.GetComponent<RewardInitializer>();
        
        initializer.setListaDeFichas(ListaDeRecompensas);
    }
}
