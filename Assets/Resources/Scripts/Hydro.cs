using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 


public enum NozzleNames { Spurt, Spray, Flow, Launch, Artillery }

public interface ICollectable
{
    public void OnCollect(); 
}


public class Hydro : MonoBehaviour
{
    public List<Nozzle> Nozzles;
    private int NozzleIndex = 0;
    public bool NozzleOn { get; set; }

    List<GameObject> FiredParticles = new List<GameObject>();

    public Sprite CurrentNozzleSprite => Nozzles[NozzleIndex].Sprite; 

    [SerializeField]
    GameObject SpurtNozzleLaunchPoint;

    private void Awake()
    {
        Nozzles = PlayerScriptableReference.PlayerSO.Nozzles;
        PlayerControls.PlayerHydro = this; 
    }

    private void Update()
    {
        if (NozzleOn && PlayerScriptableReference.PlayerSO.MoistureLevel > Nozzles[NozzleIndex].MoistureCost 
            && PlayerScriptableReference.PlayerSO.MoistureLevel > 10)
        {
            Spray(); 
        }

        foreach (GameObject go in FiredParticles)
        {
            try
            {

                if (go.TryGetComponent(out ParticleSystem PS) && !PS.isEmitting)
                {
                    FiredParticles.Remove(go);
                    Destroy(go);
                    return;
                } else if (go == null)
                    FiredParticles.Remove(go);
            }
            catch { }
        }
    }

    private void Spray()
    {
        GameObject go = Instantiate(Nozzles[NozzleIndex].Attack, SpurtNozzleLaunchPoint.transform).gameObject;
        FiredParticles.Add(go);
        float x = PlayerScriptableReference.PlayerSO.MoistureLevel;
        x = x - Nozzles[NozzleIndex].MoistureCost >= 10 ? x - Nozzles[NozzleIndex].MoistureCost : x;
        PlayerScriptableReference.PlayerSO.MoistureLevel = x; 
        PlayerCanvas.UpdateUI();
    }

    public void NozzleIndexUp()
    {
        NozzleIndex = NozzleIndex == Nozzles.Count - 1 ? 0 : NozzleIndex + 1;
        PlayerCanvas.UpdateUI();
    }

    public void NozzleIndexDown()
    {
        NozzleIndex = NozzleIndex <= 0 ? Nozzles.Count - 1 : NozzleIndex - 1;
        PlayerCanvas.UpdateUI();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(SpurtNozzleLaunchPoint.transform.position, 0.25f);
    }
}