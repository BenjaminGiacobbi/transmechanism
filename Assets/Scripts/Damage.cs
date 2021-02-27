using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] LayerMask _damageLayers;
    [SerializeField] ParticleSystem _impactParticles = null;
    [SerializeField] AudioClip _impactSound = null;
    [SerializeField] int _damageAmount = 10;
    [SerializeField] float _recoilSpeed = 35f;
    public int DamageAmount { get { return _damageAmount; } private set { _damageAmount = value; } }

    private void Start()
    {
        if (_impactParticles != null)
        {
            _impactParticles = Instantiate(_impactParticles, transform);
            _impactParticles.transform.position = transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable<int> damageable = other.gameObject.GetComponent<IDamageable<int>>();
        if (damageable != null)
        {
            damageable.Damage(DamageAmount);

            ImpactFeedback(new Vector3
                (other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z));
        }

        IRecoil recoil = other.gameObject.GetComponent<IRecoil>();
        if (recoil != null)
        {
            Vector3 origin = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 1, other.gameObject.transform.position.z);
            Vector3 direction = transform.position - origin;
            Debug.DrawRay(origin, direction, Color.red, 5f);
            if (Physics.Raycast(origin, direction, out RaycastHit hit, Mathf.Infinity))
            {
                Debug.DrawRay(hit.point, hit.normal, Color.blue, 5f);
                Debug.Log(hit.collider.transform);
                recoil.ApplyRecoil(hit.normal, _recoilSpeed);
            }
        }
    }


    //TODO no way to effective get the impact position at the moment
    public void ImpactFeedback(Vector3 feedbackPosition)
    {
        if (_impactParticles != null)
        {
            _impactParticles.transform.position = feedbackPosition;
            _impactParticles.Play();
        }

        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_impactSound, 0.25f);
        }
    }
}
