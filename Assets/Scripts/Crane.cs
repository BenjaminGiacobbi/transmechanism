using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : LevelObject
{
    [SerializeField] Transform _craneUpper = null;
    [SerializeField] Transform _spawnPoint = null;
    [SerializeField] ObjectPooler _pooler;
    [SerializeField] float _cycleTime = 3f;
    [SerializeField] float _pauseTime = 0.5f;
    [SerializeField] float _rotRange = 120f;
    private float _timer = 0;
    private float _storedTime = 0;
    private float _rotMin = 0;
    private float _rotMax = 0;
    Coroutine _rotateRoutine = null;

    // Start is called before the first frame update
    void Start()
    {
        _rotMin = _craneUpper.rotation.y - _rotRange / 2;
        Debug.Log(_rotMin);
        _rotMax = _craneUpper.rotation.y + _rotRange / 2;
        Debug.Log(_rotMax);
    }

    private void SetRotation()
    {
        float lerp = (_timer + _cycleTime) / (_cycleTime * 2);
        transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, Mathf.Lerp(_rotMin, _rotMax, lerp), transform.eulerAngles.z));
    }

    IEnumerator TimerRoutine()
    {
        _timer = 0f;
        while (true)
        {
            while (_timer < _cycleTime)
            {
                _timer += Time.deltaTime;
                if(_timer > 0 && _storedTime < 0)
                {
                    yield return new WaitForSeconds(_pauseTime);
                    SpawnScrap();
                }

                if (_timer >= _cycleTime)
                    _timer = _cycleTime;

                SetRotation();
                _storedTime = _timer;
                yield return null;
            }

            yield return new WaitForSeconds(_pauseTime);
            SpawnScrap();
            // spawn behaviour

            while (_timer > -_cycleTime)
            {
                _timer -= Time.deltaTime;
                if (_timer < 0 && _storedTime > 0)
                {
                    yield return new WaitForSeconds(_pauseTime);
                    SpawnScrap();
                }

                if (_timer <= -_cycleTime)
                    _timer = -_cycleTime;

                SetRotation();
                _storedTime = _timer;
                yield return null;
            }

            yield return new WaitForSeconds(_pauseTime);
            SpawnScrap();
        }
    }

    void SpawnScrap()
    {
        int idx = Random.Range(0, 2);
        string spawnTag = "";
        switch(idx)
        {
            case 0:
                spawnTag = "GunScrap";
                break;
            case 1:
                spawnTag = "UpperScrap";
                break;
            case 2:
                spawnTag = "BaseScrap";
                break;
        }
        GameObject spawned = _pooler.SpawnObject(spawnTag, null, _spawnPoint.position, Random.rotation);
        spawned.GetComponent<Scrap>().Init(_pooler);
    }

    public override void Activate()
    {
        Deactivate();
        _rotateRoutine = StartCoroutine(TimerRoutine());

    }

    public override void Deactivate()
    {
        if (_rotateRoutine != null)
        {
            StopCoroutine(_rotateRoutine);
            _rotateRoutine = null;
        }
    }
}
