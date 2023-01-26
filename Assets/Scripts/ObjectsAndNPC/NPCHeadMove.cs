using UnityEngine;

public class NPCHeadMove: MonoBehaviour
{

    private Vector3 BaseLookPosition;
    [SerializeField]
    private GameObject HeadTarget;
    [SerializeField]
    private float TargetRange;
    private PlayerMovement Player;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerMovement>();
        BaseLookPosition = HeadTarget.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.CheckSphere(transform.position, TargetRange, LayerMask.NameToLayer("Player")))
        {
            HeadTarget.transform.position = Player.transform.position;
        }
        else
        {
            HeadTarget.transform.position = BaseLookPosition;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, TargetRange);
    }
#endif
}
