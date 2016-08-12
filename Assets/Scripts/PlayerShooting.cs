using UnityEngine;
using System.Collections;


public class PlayerShooting : MonoBehaviour
{
    public GameObject Body;                         ///Ссылка на оруж

    public int damagePerShot = 20;                  // Уорона за выстрел
    public float timeBetweenBullets = 0.15f;        // Время между каждым выстрелом.
    public float range = 100f;                      // Расстояние выстрела.

    float timer;                                    // Таймер для определения, когда стрелять.
    private Ray shootRay;                                   // Луч из оружия, который выпущен вперед.
    private RaycastHit shootHit;                            // Луч поал, получаем информацию
    //int shootableMask;                              // Индекс слоя в котором попадаем во врагов
    private ParticleSystem _gunParticles;                    // Эффект выстрела
    private LineRenderer _gunLine;                           // Линия выстрела
    private AudioSource _gunAudio;                           // Звук выстрела
    private Light _gunLight;                                 // Свет выстрела
    public Light faceLight;
    float effectsDisplayTime = 0.2f;

    HealthHelper _parent;
    void Awake()
    {
        // Берем индекс слоя Shootable
        //shootableMask = LayerMask.GetMask("Shootable");

        _gunParticles = GetComponent<ParticleSystem>();
        _gunLine = GetComponent<LineRenderer>();
        _gunAudio = GetComponent<AudioSource>();
        _gunLight = GetComponent<Light>();


        _parent = GetComponentInParent<HealthHelper>();
    }


    void Update()
    {
        // Добавьте время, так как обновление было в прошлом Update.
        timer += Time.deltaTime;



        // If the timer has exceeded the proportion of timeBetweenBullets that the effects should be displayed for...
        if (timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects();
        }

    }

    public void DisableEffects()
    {
        // Disable the line renderer and the light.
        _gunLine.enabled = false;
        faceLight.enabled = false;
        _gunLight.enabled = false;
    }

    public void StartShoot()
    {
        if (timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot();
        }
    }
    void Shoot()
    {
        timer = 0f;

       _gunAudio.Play();

        // Включаем всвет
        _gunLight.enabled = true;
        faceLight.enabled = true;

        _gunParticles.Stop();
        _gunParticles.Play();

        // Линия выстрела
        _gunLine.enabled = true;
        _gunLine.SetPosition(0, transform.position);
        // Настройка луча
        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if (Physics.Raycast(shootRay, out shootHit, range))
        {
            if (shootHit.collider.GetComponentInParent<HealthHelper>() &&
                shootHit.collider.GetComponent<HeadHeplper>())
            {
                GameObject blood = Instantiate(Resources.Load("BloodHead"), shootHit.point, Quaternion.identity) as GameObject;
                Destroy(blood, 1);
                shootHit.collider.GetComponentInParent<HealthHelper>().GetDamage(200, _parent);
            }
            else if (shootHit.collider.GetComponentInParent<HealthHelper>())
            {
                GameObject blood = Instantiate(Resources.Load("BloodBody"), shootHit.point, Quaternion.identity) as GameObject;
                Destroy(blood, 1);
                shootHit.collider.GetComponentInParent<HealthHelper>().GetDamage(damagePerShot, _parent);


            }
            else if (shootHit.collider.GetComponent<Rigidbody>())
            {
                shootHit.collider.GetComponent<Rigidbody>().AddForce(transform.forward * 300);
            }

            // Рисуем линию
            _gunLine.SetPosition(1, shootHit.point);
        }
        // Если не попали
        else
        {
            _gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
        }
    }


    public void Drop()
    {
        StartCoroutine(WaitDrop());
    }

    IEnumerator WaitDrop()
    {
        yield return new WaitForSeconds(0.3f);

        Body.transform.SetParent(null);
        Body.GetComponent<Collider>().enabled = true;
        Body.GetComponent<Rigidbody>().isKinematic = false;
        Body.GetComponent<Rigidbody>().AddForce(Random.insideUnitSphere * 200);
    }
}