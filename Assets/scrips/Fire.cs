using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _FireRange=50;// serField  поле доступно через инспектор но не доступно через другий компоненты
    [SerializeField] private ParticleSystem _Effect;
    [SerializeField] private ParticleSystem _ShootEffect;    
    [SerializeField] private Camera _Cam;
    [SerializeField] private GameObject _HitEffect;
    [SerializeField] private float _Damage=100;
    [SerializeField] private GameObject _Destroed ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }
    private void changeBulletShoot(ref Ray ray,RaycastHit hit)
    {
        Vector3 v1 = ray.direction - ray.origin;
        v1.x += v1.x * Random.Range(-0.1f, 0.1f);
        v1.y += v1.y * Random.Range(-0.1f, 0.1f);
        v1.z += v1.z * Random.Range(-0.1f, 0.1f);
        
        ray.origin = hit.point;
        ray.direction += ray.origin+v1;
    }
    
    private void HitEffetPlay(RaycastHit hit)
    {
        ParticleSystem shootEffectNow = Instantiate(_ShootEffect, hit.point, Quaternion.LookRotation(hit.normal));
       
        Destroy(shootEffectNow, 2);
    }
    private void Decaly(RaycastHit hit)
    {
        GameObject bulletTrase = Instantiate(_HitEffect, hit.point, Quaternion.LookRotation(hit.normal));//лучше повесить эффект на обект и вызывать его отсюда??
        Destroy(bulletTrase, 10);
        
        
    }
    private void Shoot()
    {        
        _Effect.Play();// проигрывание эфекта стрельбы
        RaycastHit hit;
        Ray ray = new Ray(_Cam.transform.position, _Cam.transform.forward * _FireRange);
        bool lockWhile = true;
        while (lockWhile)
        {
            lockWhile = Physics.Raycast(ray, out hit);
            switch (hit.transform.name)// незнаю по какому параметру сортировать выбраный объект поэтому по имени
            {
                case "Wood":
                    //декали
                    Decaly(hit);
                    HitEffetPlay(hit);
                    
                    //???????????????? расчет начальной точнки спавна нового луча со здвигом на шаг в перед
                    Vector3 endPoint = _Cam.transform.forward * _FireRange;
                    Vector3 start = hit.point + (endPoint - hit.point) / 100;
                    ray = new Ray(start, endPoint);
                    //изменение траектории луча
                    changeBulletShoot(ref ray, hit);
                    ray.origin = start;
                    break;
                case "Jug":
                    //эффект попадания в вазу
                    HitEffetPlay(hit);
                    //спавним  обект вместо вазы
                    Vector3 spawnPoint= hit.transform.position;                    
                    Instantiate(_Destroed,spawnPoint, hit.transform.rotation);
                    // уничтожаем вазу
                    Destroy(hit.transform.gameObject);
                    //???????????????? расчет начальной точнки спавна нового луча на шаг в перед
                     endPoint = _Cam.transform.forward * _FireRange;
                     start = hit.point + (endPoint - hit.point) / 100;
                    ray = new Ray(start, endPoint);
                    //изменение траектории луча
                    changeBulletShoot(ref ray,hit);
                    ray.origin = start;
                    break;
                case "Bucket":
                    //эффект от попадания
                    HitEffetPlay(hit);
                    GameObject bulletTrase = Instantiate(_HitEffect, hit.point, Quaternion.LookRotation(hit.normal),hit.transform);//лучше повесить эффект на обект и вызывать его отсюда??
                    Destroy(bulletTrase, 10);
                    hit.rigidbody.AddForce(-hit.normal*_Damage);
                    lockWhile = false;
                    break;
                case "Rock":
                    Decaly(hit);
                    HitEffetPlay(hit);
                    // рефлект отражение выстрела
                    Vector3 fallingVect = hit.point - ray.origin;
                    float angl = Vector3.Angle(fallingVect, hit.normal);
                    if (Random.Range(90,angl)+25 > Random.Range(90, 180))
                    {                        
                        Vector3 reflectVect = Vector3.Reflect(fallingVect, hit.normal);                        
                        ray = new Ray(hit.point, reflectVect);
                        changeBulletShoot(ref ray, hit);
                    }
                    else
                        lockWhile = false;
                    break;
                case "Mug(Clone)":
                    Decaly(hit);
                    HitEffetPlay(hit);
                    changeBulletShoot(ref ray, hit);
                    break;
                default:
                    lockWhile = false;
                    break;     
            }     
        }
    }
}
