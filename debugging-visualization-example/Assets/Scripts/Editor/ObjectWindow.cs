using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Reflection;

public class ObjectWindow : EditorWindow
{

    private SerializeSubobjects _currentTarget;
    private int _margin = 32;
    private int _windowHeight = 128;
    private int _windowWidth = 256;
    private float _panX = 0;
    private float _panY = 0;

    private Texture2D _windowTexture;
    private bool _initted = false;
    private GUIStyle _windowInactive;
    private GUIStyle _windowActive;
    private GUIStyle _memberText;

    [MenuItem("Window/Object Visualization")]
    static void Create()
    {
        ObjectWindow window = (ObjectWindow)EditorWindow.GetWindow(typeof(ObjectWindow));
        window.Show();
    }

    void Init()
    {
        _initted = true;
        _windowInactive = new GUIStyle();
        _windowInactive.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;
        _windowInactive.alignment = TextAnchor.UpperCenter;
        _windowInactive.normal.textColor = Color.white;
        _windowInactive.border = new RectOffset(_margin / 4, _margin / 4, _margin / 4, _margin / 4);
        _windowInactive.padding = new RectOffset(_margin / 2, _margin / 2, _margin / 2, _margin / 2);
        _windowInactive.fontSize = 14;
        _windowInactive.richText = true;

        _windowActive = new GUIStyle(_windowInactive);
        _windowActive.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node3.png") as Texture2D;

        _memberText = new GUIStyle();
        _memberText.normal.textColor = Color.white;
        _memberText.richText = true;
    }

    void Update()
    {
        if (_initted == false)
        {
            Init();
        }
        var transform = Selection.activeTransform;
        if (transform != null && _currentTarget == null)
        {
            var testAgent = transform.GetComponent<SerializeSubobjects>();
            _currentTarget = testAgent;
        }
        Repaint();
    }

    public void OnGUI()
    {
        Handles.BeginGUI();
        HandlePan();
        GUI.BeginGroup(new Rect(_panX, _panY, int.MaxValue, int.MaxValue));
        if (_currentTarget != null)
        {
            DrawObjects();
        }
        GUI.EndGroup();
        Handles.EndGUI();
    }

    private void HandlePan()
    {
        if (Event.current.type == EventType.MouseDrag)
        {
            _panX += Event.current.delta.x;
            _panY += Event.current.delta.y;
        }
    }

    private void DrawObjects()
    {
        BeginWindows();
        var objects = _currentTarget.GetSerializableObjects();
        for (int i = 0; i < objects.Count; i++)
        {
            var obj = objects[i];
            var pos = new Rect(_margin + i * (_margin + _windowWidth), _margin, _windowWidth, _windowHeight);
            DrawObject(obj, pos);
        }
        EndWindows();
    }

    private void DrawObject(object obj, Rect pos)
    {
        var strategyName = $"<b>{obj.GetType().ToString()}</b>";
        var fieldInfos = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        pos.height = 2 * _margin + fieldInfos.Length * _memberText.lineHeight;
        GUI.Box(pos, strategyName, obj == _currentTarget.GetCurrentObject() ? _windowActive : _windowInactive);
        var innerBox = new Rect(pos);
        innerBox.y += _margin * 1.25f;
        innerBox.x += _margin / 2;
        innerBox.width -= _margin;
        innerBox.height = pos.height;
        GUILayout.BeginArea(innerBox);
        GUILayout.BeginVertical();
        foreach (var info in fieldInfos)
        {
            var value = info.GetValue(obj);
            var valueType = value.GetType();
            var valueAsType = Convert.ChangeType(value, valueType);
            var valueText = valueAsType == null ? "null" : Serialize(valueAsType);
            var text = $"<b>{info.Name}</b> : {valueText}";
            GUILayout.Label(text, _memberText);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }

    private string Serialize(object o)
    {
        if (o.GetType() == typeof(Transform)) return (o as Transform).name;
        return o.ToString();
    }

}
