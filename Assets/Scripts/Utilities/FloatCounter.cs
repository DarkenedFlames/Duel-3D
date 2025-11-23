

public class FloatCounter
{
    float _value;
    public float Value => _value;

    readonly float _min;
    readonly float _max;

    readonly bool _inclusive;
    readonly bool _resetToMax;

    public bool Expired => _inclusive ? _value <= _min : _value < _min;
    public bool Exceeded => _inclusive ? _value >= _max : _value > _max;

    public FloatCounter(float initial, float min = float.MinValue, float max = float.MaxValue,  bool inclusive = true, bool resetToMax = true)
    {
        _max = max;
        _min = min;
        _value = initial;
        _inclusive = inclusive;
        _resetToMax = resetToMax;
    }

    public void Increment() => _value++;
    public void Decrement() => _value--;
    public void Increase(float delta) => _value += delta;
    public void Decrease(float delta) => _value -= delta;
    public void Reset() 
    {
        if (_resetToMax) _value = _max;
        else _value = _min;
        
    }
}