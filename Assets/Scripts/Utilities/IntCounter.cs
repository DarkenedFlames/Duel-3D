using UnityEngine;

public class IntCounter
{
    int _value;
    public int Value => _value;
    readonly int _period;
    public bool Expired => _value <= Mathf.Epsilon;

    public IntCounter(int period)
    {
        _period = period;
        _value = period;
    }

    public void Decrement() => _value--;
    public void Decrease(int delta) => _value -= delta;
    public void Reset() => _value = _period;

}