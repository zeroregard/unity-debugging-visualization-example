using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SerializeSubobjects
{
	List<object> GetSerializableObjects();
	object GetCurrentObject();
}

public class TestAgent : MonoBehaviour, SerializeSubobjects
{
    private List<IStrategy> _strategies = new List<IStrategy>();
    private object _currentStrategy;
    private float _switchTime = 1;
    private float _dt = 0;
    private int _iterator = 0;

    void Start()
    {
        _strategies.Add(new MoveStrategy());
        _strategies.Add(new MoveStrategy());
        _strategies.Add(new MoveStrategy());
        _strategies.Add(new MoveStrategy());
        _strategies.Add(new MoveStrategy());
    }

    void Update()
    {
        foreach (var s in _strategies)
        {
            s.Act();
        }
        _dt += Time.deltaTime;
        if (_dt >= _switchTime)
        {
            _dt = 0;
            _iterator = (int)Mathf.Repeat(_iterator + 1, _strategies.Count);
            _currentStrategy = _strategies[_iterator];
        }
    }

    public List<object> GetSerializableObjects()
    {
        return _strategies.ConvertAll(x => (object)x);
    }

    public object GetCurrentObject()
    {
        return _currentStrategy;
    }
}
