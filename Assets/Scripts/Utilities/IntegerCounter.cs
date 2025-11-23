public class IntegerCounter
{
    int _value;
    public int Value => _value;

    readonly int _min;
    readonly int _max;

    readonly bool _inclusive;
    readonly bool _resetToMax;

    public bool Expired => _inclusive ? _value <= _min : _value < _min;
    public bool Exceeded => _inclusive ? _value >= _max : _value > _max;

    public IntegerCounter(int initial, int min = int.MinValue, int max = int.MaxValue,  bool inclusive = true, bool resetToMax = true)
    {
        _max = max;
        _min = min;
        _value = initial;
        _inclusive = inclusive;
        _resetToMax = resetToMax;
    }

    public void Increment() => _value++;
    public void Decrement() => _value--;
    public void Increase(int delta) => _value += delta;
    public void Decrease(int delta) => _value -= delta;
    public void Reset() 
    {
        if (_resetToMax) _value = _max;
        else _value = _min;
        
    }
}