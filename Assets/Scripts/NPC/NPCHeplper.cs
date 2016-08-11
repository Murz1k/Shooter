using UnityEngine;
using System.Collections;
using System.Linq;

public class NPCHeplper : MonoBehaviour
{
    public float FireRange = 10;

    HealthHelper _target;

    public HealthHelper Target
    {
        get { return _target; }
        set { _target = value; }
    }


    HealthHelper _healthHelper;
    PlayerShooting _gun;

    // Use this for initialization
    void Start()
    {
        _healthHelper = GetComponent<HealthHelper>();
        _healthHelper = GetComponent<HealthHelper>();
        _gun = GetComponentInChildren<PlayerShooting>();
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        HealthHelper[] targets = GameObject.FindObjectsOfType<HealthHelper>().Where(p => p.Group != _healthHelper.Group && !p.Dead).ToArray();

        if (targets.Length == 0)
        {
            GetComponent<Animator>().SetTrigger("Winner");
        }
        else
        {
            _target = targets[Random.Range(0, targets.Length)];

            if (!_healthHelper.Dead)
            {
                yield return new WaitForSeconds(5);
                StartCoroutine(Timer());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!_target ||
            _healthHelper.Dead ||
            _target.Dead)
            return;

        if (FireRange > Vector3.Distance(transform.position, _target.transform.position))
        {
            Vector3 targetPos = new Vector3(_target.transform.position.x, 0, _target.transform.position.z);
            transform.LookAt(targetPos);
            float height = _target.GetComponent<CapsuleCollider>().height;
            Vector3 firePosition = new Vector3(_target.transform.position.x, height, _target.transform.position.z);

            Vector3 randomeSph = Random.insideUnitSphere * 1.5f;

            _gun.Body.transform.LookAt(firePosition + randomeSph);
            _gun.StartShoot();
        }
    }
}
