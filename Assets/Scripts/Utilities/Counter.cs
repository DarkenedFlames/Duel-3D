using UnityEngine;

public class Counter
{
    float _value;
    public float Value => _value;
    readonly float _period;
    public bool Expired => _value <= Mathf.Epsilon;



    public Counter(float period)
    {
        _period = period;
        _value = period;
    }

    public void Decrement() => _value--;
    public void Decrease(float delta) => _value -= delta;
    public void Reset() => _value = _period;

}