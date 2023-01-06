using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum NozzleNames { Spurt, Dash, Flow, Launch, Artillery }


public class Hydro : MonoBehaviour
{
    [SerializeField]
    private GameObject _hydroLevel; 
    public static GameObject HydroLevel; 
    
    public List<Nozzle> Nozzles;
    private int NozzleIndex = 0;
    private bool nozzleOn = false; 
    public bool NozzleOn {
        get 
        {
            return nozzleOn; 
        } 
        set 
        {
            nozzleOn = value; 
        } 
    }

    private List<GameObject> NozzleList = new List<GameObject>();

    public Sprite CurrentNozzleSprite => Nozzles[NozzleIndex].Sprite; 

    [SerializeField]
    private GameObject SpurtNozzleLaunchPoint;

    [SerializeField]
    private GameObject CurrentNozzle;

    [SerializeField][Range(0.0f, 2.5f)]
    private float FillZoneSize;
    [SerializeField]
    private LayerMask WellLayer;
    [SerializeField]
    private WaterInteractableBehavior ClosestWell;
    private PlayerAnimations animations;

    [SerializeField]
    public float HoverWalkCost; 

    private void Awake()
    {
        Nozzles = PlayerScriptableReference.PlayerSO.Nozzles;
        PlayerControls.PlayerHydro = this;
        HydroLevel = _hydroLevel;
        //Activation of Set MoistureLevel property without trying to change the moisture level  
        PlayerScriptableReference.PlayerSO.MoistureLevel = PlayerScriptableReference.PlayerSO.MoistureLevel;
        animations = GetComponent<PlayerAnimations>(); 

        foreach (Nozzle go in Nozzles)
        {
            GameObject g = Instantiate(go.Attack, SpurtNozzleLaunchPoint.transform);
            g.SetActive(false);
            NozzleList.Add(g); 
        }

        CurrentNozzle = NozzleList[0]; 

    }

    private void FixedUpdate()
    {
        if (SprayBool(nozzleOn))
        {
            Spray();
        }
        
        NozzleEffect(); 
        
        animations.Spray(AttackNozzle() && SprayBool(nozzleOn));
    }

    public void NozzleEffect()
    {
        CurrentNozzle.SetActive(!inFillRangeOfWell() && PlayerScriptableReference.PlayerSO.MoistureLevel > 1 && nozzleOn);
    }

    private bool AttackNozzle()
    {
        return (CurrentNozzle.name.Contains("Spurt") || CurrentNozzle.name.Contains("Flow")); 
    }

    private bool inFillRangeOfWell()
    {
        CharacterController Controller = GetComponent<CharacterController>();
        var inRange = Physics.CheckSphere(Controller.bounds.center + HalfVector.HalfForward, FillZoneSize, WellLayer);
        if (!inRange)
        {
            ClosestWell = null; 
            return inRange; 
        }

        Physics.OverlapSphere(Controller.bounds.center + HalfVector.HalfForward, FillZoneSize, WellLayer)[0].TryGetComponent(out WaterInteractableBehavior WatterCollection);
        ClosestWell = WatterCollection;

        return inRange; 
    }

    private void CalculateHoverWalk(float HoverCost)
    {
        PlayerMovement Player = GetComponent<PlayerMovement>(); 
        float x = PlayerScriptableReference.PlayerSO.MoistureLevel;
        x = x - HoverCost >= 0 ? x - HoverCost : x;
        PlayerScriptableReference.PlayerSO.MoistureLevel = x;
        PlayerCanvas.UpdateUI();
        if(x - HoverCost < 0)
        {
            Player.NormalJumpEnd(); 
            return;
        }

        Player.Hover();

    }

    public void HoverWalk(bool isOn)
    {
        if (isOn) CalculateHoverWalk(HoverWalkCost);
    }


    private void Spray()
    {
        float x = PlayerScriptableReference.PlayerSO.MoistureLevel;
        x = x - Nozzles[NozzleIndex].MoistureCost >= 0 ? x - Nozzles[NozzleIndex].MoistureCost : x;
        PlayerScriptableReference.PlayerSO.MoistureLevel = x;
        PlayerCanvas.UpdateUI();

        if (AttackNozzle() && inFillRangeOfWell())
        {
            ClosestWell.ManualFill();
        }
    }

    private bool SprayBool(bool SprayPress)
    {
        return nozzleOn && PlayerScriptableReference.PlayerSO.MoistureLevel > Nozzles[NozzleIndex].MoistureCost
             && PlayerScriptableReference.PlayerSO.MoistureLevel > 0 && SprayPress;
    }

    public void NozzleIndexUp()
    {
        CurrentNozzle.SetActive(false);
        NozzleIndex = NozzleIndex == Nozzles.Count - 1 ? 0 : NozzleIndex + 1;
        CurrentNozzle = NozzleList[NozzleIndex]; 
        PlayerCanvas.UpdateUI();
    }

    public void NozzleIndexDown()
    {
        CurrentNozzle.SetActive(false);
        NozzleIndex = NozzleIndex <= 0 ? Nozzles.Count - 1 : NozzleIndex - 1;
        CurrentNozzle = NozzleList[NozzleIndex];
        PlayerCanvas.UpdateUI();
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; 
        Gizmos.DrawWireSphere(GetComponent<CharacterController>().bounds.center + HalfVector.HalfForward, FillZoneSize);
        Gizmos.color = Color.black; 
        Gizmos.DrawWireSphere(SpurtNozzleLaunchPoint.transform.position, 0.25f);
    }
#endif
}