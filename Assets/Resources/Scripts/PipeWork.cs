using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeWork : MonoBehaviour
{
    Pipe Pipe0, Pipe1; 

    void Start()
    {
        Pipe0 = transform.GetChild(0).gameObject.GetComponent<Pipe>(); 
        Pipe1 = transform.GetChild(1).gameObject.GetComponent<Pipe>(); 
    }

    public void TransportPlayerBetweenPipes(bool _isPipe0)
    {
        if (_isPipe0)
        {
            ToPipe0();
            return; 
        }
        ToPipe1();
    }

    private void ToPipe0()
    {
        Pipe1.TeleportPlayer();
    }

    private void ToPipe1()
    {
        Pipe0.TeleportPlayer(); 
    }

}
