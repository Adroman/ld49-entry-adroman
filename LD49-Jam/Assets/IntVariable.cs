using UnityEngine;

[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    [InspectorName("Value")]
    [SerializeField]
    // ReSharper disable once InconsistentNaming
    private int _value;

    public GameEvent OnValueChanged;
    
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            if (OnValueChanged != null)
                OnValueChanged.RaiseEvent();
        }
    }
}