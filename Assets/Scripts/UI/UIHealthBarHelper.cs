using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIHealthBarHelper : MonoBehaviour
{
    Transform _nPC;
    public Transform NPC
    {
        get { return _nPC; }
        set
        {
            _nPC = value;
            _healthHelper = NPC.GetComponent<HealthHelper>();
            _slider = GetComponentInChildren<Slider>();
            _slider.maxValue = _healthHelper.MaxHealth;
        }
    }

    Slider _slider;
    HealthHelper _healthHelper;

    Text _kills;
    // Use this for initialization
    void Start()
    {
        _kills = transform.FindChild("Kills").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!NPC)
            return;

        Vector3 npcPos = new Vector3(NPC.position.x, NPC.position.y + 2.2f, NPC.position.z);
        GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(npcPos);
        if (_slider)
            _slider.value = _healthHelper.Health;

        _kills.text = _healthHelper.Kills.ToString();
    }

    public void DisableSlider()
    {
        Destroy(_slider.gameObject);
    }
}
