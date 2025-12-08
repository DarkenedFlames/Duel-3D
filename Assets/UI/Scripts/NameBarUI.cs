using UnityEngine;
using TMPro;

public class NameBarUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    void Awake() => nameText = GetComponentInChildren<TextMeshProUGUI>();
    public void Initialize(Character owner) =>  nameText.text = owner.gameObject.name;
    
}