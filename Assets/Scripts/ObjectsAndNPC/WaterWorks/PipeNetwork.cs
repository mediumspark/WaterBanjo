using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeNetwork : MonoBehaviour
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
        PlayerControls.TeleportPlayer(Pipe1.transform.GetChild(0));
    }

    private void ToPipe1()
    {
        PlayerControls.TeleportPlayer(Pipe0.transform.GetChild(0)); 
    }

}
