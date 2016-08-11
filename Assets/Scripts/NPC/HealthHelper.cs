using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class HealthHelper : MonoBehaviour
{

    public int MaxHealth = 100;
    public int Health = 100;
    public int Group = 0;


    public bool isDynamicHealthBarCreate = true;
    private bool _dead;
    public bool Dead
    {
        get { return _dead; }
        set { _dead = value; }
    }


    public int Kills { get; private set; }

    private UIHealthBarHelper _uIHealthBarHelper;
    private PlayerShooting _playerShooting;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private Text _HealthText;

    // Use this for initialization
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _playerShooting = GetComponentInChildren<PlayerShooting>();
        _HealthText = GameObject.Find("HealthText").GetComponent<Text>();

        if (isDynamicHealthBarCreate)
        {
            GameObject healtBarSlider = Instantiate(Resources.Load("HealtBarSlider"), Vector3.zero, Quaternion.identity) as GameObject;
            healtBarSlider.transform.SetParent(GameObject.Find("GameUI").transform);
            _uIHealthBarHelper = healtBarSlider.GetComponent<UIHealthBarHelper>();
            _uIHealthBarHelper.NPC = transform;
        }
    }
    
    public void GetDamage(int damage, HealthHelper killer)
    {
        if (Dead)
            return;

        Health -= damage;
        if (Health > 0)
            _HealthText.text = Health.ToString();
        else
            _HealthText.text = "0";

        if (Health <= 0)
        {
            Dead = true;
            killer.Kills += 1;
            if(_playerShooting)
            _playerShooting.Drop();
            if (_animator)
                _animator.SetBool("Dead",true);
            if (_navMeshAgent)
                _navMeshAgent.enabled = false;

            if (_uIHealthBarHelper)
                _uIHealthBarHelper.DisableSlider();
        }

    }
}
