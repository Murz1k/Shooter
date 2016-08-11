using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class CustomMove : MonoBehaviour
{
    public byte rotateSpeed = 3;

    private Animator _animator;

    private PlayerShooting _gun;

    private byte _maxBullets = 200;

    private byte _bulletCount;

    private Camera _camera;

    private float _lastMousePosition;

    [SerializeField]
    private float mouseSensitivity = 5.0f;

    private float _verticalRotation = 0.0f;

    public float upDownRange = 60.0f;
    private Text _BulletText;

    void Start()
    {
        _camera = Camera.main;
        _animator = GetComponent<Animator>();
        _gun = GetComponentInChildren<PlayerShooting>();
        _BulletText = GameObject.Find("BulletText").GetComponent<Text>();
        _bulletCount = _maxBullets;
        _BulletText.text = _bulletCount.ToString();
    }

    void Update()
    {
        if (!_animator.GetBool("Dead"))
        {
            bool fire = Input.GetButton("Fire1");
            if (Input.GetKeyDown(KeyCode.R))
                _bulletCount = _maxBullets;
            Fire(fire);
        }
    }
    void Fire(bool IsFire)
    {
        _animator.SetBool("Fire", IsFire);
        if(IsFire && _bulletCount!=0)
        {
            _gun.StartShoot();
            _bulletCount--;
            _BulletText.text = _bulletCount.ToString();
            print(_bulletCount);
        }
    }
}
