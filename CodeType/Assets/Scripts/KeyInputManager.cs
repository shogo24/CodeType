using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyInputManager : MonoBehaviour
{
    [System.Serializable]
    public class KeyBinding
    {
        public Key key;
        public KeyAnimatedObject target;
    }

    public List<KeyBinding> keyBindings;
    private Dictionary<Key, KeyAnimatedObject> keyMap;

    void Awake()
    {
        keyMap = new Dictionary<Key, KeyAnimatedObject>();
        foreach (var binding in keyBindings)
        {
            if (!keyMap.ContainsKey(binding.key))
            {
                keyMap.Add(binding.key, binding.target);
            }
        }
    }

    void Update()
    {
        if (Keyboard.current == null) return;

        foreach (var pair in keyMap)
        {
            var keyControl = Keyboard.current[pair.Key];
            if (keyControl != null && keyControl.wasPressedThisFrame)
            {
                pair.Value.Play();
            }
        }
    }
}
