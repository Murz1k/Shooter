using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ConfigurableJoint))]
[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float mouseSensitivity = 15f;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float thrusterFuelBurnSpeed = 1f;
    [SerializeField]
    private float thrusterFuelRegenSpeed = 0.3f;
    private float thrusterFuelAmount = 1f;

    private byte _maxBullets = 200;

    private byte _bulletCount;

    public float GetThrusterFuelAmount()
    {
        return thrusterFuelAmount;
    }

    [SerializeField]
    private LayerMask environmentMask;

    [Header("Spring settings:")]
    [SerializeField]
    private float jointSpring = 20f;
    [SerializeField]
    private float jointMaxForce = 40f;

    // Component caching

    private PlayerShooting _gun;
    private PlayerMotor motor;
    private ConfigurableJoint joint;
    private Animator _animator;
    private Text _BulletText;

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        joint = GetComponent<ConfigurableJoint>();
        _animator = GetComponent<Animator>();
        _gun = GetComponentInChildren<PlayerShooting>();
        _BulletText = GameObject.Find("BulletText").GetComponent<Text>();
        _bulletCount = _maxBullets;
        _BulletText.text = _bulletCount.ToString();

        SetJointSettings(jointSpring);
    }

    void Update()
    {
        //if (PauseMenu.IsOn)
        //return;


        if (!_animator.GetBool("Dead"))
        {
            bool fire = Input.GetButton("Fire1");
            if (Input.GetKeyDown(KeyCode.R))
                _bulletCount = _maxBullets;
            Fire(fire);
        }

        //Setting target position for spring
        //This makes the physics act right when it comes to
        //applying gravity when flying over objects
        RaycastHit _hit;
        if (Physics.Raycast(transform.position, Vector3.down, out _hit, 100f, environmentMask))
        {
            joint.targetPosition = new Vector3(0f, -_hit.point.y, 0f);
        }
        else
        {
            joint.targetPosition = new Vector3(0f, 0f, 0f);
        }

        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxis("Horizontal");
        float _zMov = Input.GetAxis("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

        // Animate movement
        _animator.SetFloat("ForwardVelocity", _zMov);

        //Apply movement
        motor.Move(_velocity);

        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * mouseSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * mouseSensitivity;

        //Apply camera rotation
        motor.RotateCamera(_cameraRotationX);

        // Calculate the thrusterforce based on player input
        Vector3 _thrusterForce = Vector3.zero;
        if (Input.GetButton("Jump") && thrusterFuelAmount > 0f)
        {
            thrusterFuelAmount -= thrusterFuelBurnSpeed * Time.deltaTime;

            if (thrusterFuelAmount >= 0.01f)
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetJointSettings(0f);
            }
        }
        else
        {
            thrusterFuelAmount += thrusterFuelRegenSpeed * Time.deltaTime;
            SetJointSettings(jointSpring);
        }

        thrusterFuelAmount = Mathf.Clamp(thrusterFuelAmount, 0f, 1f);

        // Apply the thruster force
        motor.ApplyThruster(_thrusterForce);

    }

    private void SetJointSettings(float _jointSpring)
    {
        joint.yDrive = new JointDrive
        {
            positionSpring = _jointSpring,
            maximumForce = jointMaxForce
        };
    }

    void Fire(bool IsFire)
    {
        _animator.SetBool("Fire", IsFire);
        if (IsFire && _bulletCount != 0)
        {
            _gun.StartShoot();
            _bulletCount--;
            _BulletText.text = _bulletCount.ToString();
            print(_bulletCount);
        }
    }

}