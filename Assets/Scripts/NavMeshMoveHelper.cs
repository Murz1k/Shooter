using UnityEngine;
using System.Collections;

public class NavMeshMoveHelper : MonoBehaviour
{
    public Transform EndPoint;


    NavMeshAgent _navMeshAgent;
    HealthHelper _healthHelper;
    NPCHeplper _nPCHeplper;
    // Use this for initialization
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _healthHelper = GetComponent<HealthHelper>();
        _nPCHeplper = GetComponent<NPCHeplper>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_nPCHeplper.Target.Dead ||
            !_nPCHeplper.Target ||
            _healthHelper.Dead)
            return;
        Debug.Log(" Update()");
        // if (_navMeshAgent.isOnOffMeshLink)
        GetComponent<Animator>().SetBool("OffMeshLink", _navMeshAgent.isOnOffMeshLink);
        _navMeshAgent.SetDestination(_nPCHeplper.Target.transform.position);
    }
}
