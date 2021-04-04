using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class JunkGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _objectsToSpawn;
    [SerializeField] int _autoSpawnNumber;
    [SerializeField] BoxCollider _collider = null;
    private int _arraySize;
    private List<SpawnedObject> _spawnedObjects;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        _arraySize = _objectsToSpawn.Length;
        _spawnedObjects = new List<SpawnedObject>();
    }

    // returns a json representation of the current equipment array
    private string ConvertToJson()
    {
        if (_spawnedObjects.Count < 1)
            return "FILE CREATION ERROR: EMPTY ARRAY";

        List<WriteObject> _writeObjects = new List<WriteObject>();
        foreach(SpawnedObject obj in _spawnedObjects)
        {
            _writeObjects.Add(new WriteObject(
                        obj._transform.position.x, obj._transform.position.y, obj._transform.transform.position.z,
                        obj._transform.eulerAngles.x, obj._transform.eulerAngles.z, obj._transform.eulerAngles.z,
                        obj._prefabName));
        }

        // constructs the json form of the inventory data
        string fullJsonString = null;
        for (int item = 0; item < _writeObjects.Count; item++)
        {
            fullJsonString += JsonUtility.ToJson(_writeObjects[item]);
            if (item != _writeObjects.Count - 1)
                fullJsonString += ",";
        }

        // returns
        _writeObjects = null;
        return fullJsonString;
    }

    public void AutoSpawn()
    {
        for(int i = 0; i < _autoSpawnNumber; i++)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        // informs designer about the conditions of save usage
        if (_objectsToSpawn.Length < 1)
        {
            
            Debug.Log("Array is empty. Cannot spawn from empty array");
            return;
        }
        int random = Random.Range(0, _arraySize - 1);
        GameObject spawned = Instantiate(_objectsToSpawn[random], transform);
        float x = Random.Range(_collider.bounds.min.x, _collider.bounds.max.x);
        float y = Random.Range(_collider.bounds.min.y, _collider.bounds.max.y);
        float z = Random.Range(_collider.bounds.min.z, _collider.bounds.max.z);
        float rotx = Random.Range(0, 360);
        float roty = Random.Range(0, 360);
        float rotz = Random.Range(0, 360);
        spawned.transform.position = new Vector3(x, y, z);
        spawned.transform.rotation = Quaternion.Euler(rotx, roty, rotz);
        _spawnedObjects.Add(new SpawnedObject(spawned.transform, _objectsToSpawn[random].name));
        
    }

    // retrieves Json form of inventory data and saves it to a file of specified path
    public void SaveToFile(string path)
    {
        StreamWriter streamWriter = new StreamWriter(path);
        streamWriter.Write(ConvertToJson());
        streamWriter.Close();
    }

    public void RecreateSpawns(string path)
    {
        StreamReader reader = new StreamReader(path);
        string jsonContents = reader.ReadToEnd();
        Debug.Log(jsonContents);
        WriteObject[] writeObjects = JsonHelper.GetJsonArray<WriteObject>(jsonContents);
        reader.Close();
        Debug.Log(writeObjects);

        foreach (WriteObject obj in writeObjects)
        {
            Debug.Log("SpawnPrefabs/" + obj._prefabName);
            GameObject newObj = Instantiate(Resources.Load("SpawnPrefabs/" + obj._prefabName)) as GameObject;
            newObj.transform.SetParent(transform);
            newObj.transform.position = new Vector3(obj._posX, obj._posY, obj._posZ);
            newObj.transform.rotation = Quaternion.Euler(new Vector3(obj._rotX, obj._rotY, obj._rotZ));
        }
    }
}

public class SpawnedObject
{
    public Transform _transform;
    public string _prefabName;

    public SpawnedObject(Transform transform, string prefabName)
    {
        _transform = transform;
        _prefabName = prefabName;
    }
}

[System.Serializable]
public class WriteObject
{
    public float _posX, _posY, _posZ;
    public float _rotX, _rotY, _rotZ;
    public string _prefabName;

    public WriteObject(float px, float py, float pz, float rx, float ry, float rz, string prefabName)
    {
        _posX = px;
        _posY = py;
        _posZ = pz;
        _rotX = rx;
        _rotY = ry;
        _rotZ = rz;
        _prefabName = prefabName;
    }
}
