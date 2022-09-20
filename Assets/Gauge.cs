using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Gauge : MonoBehaviour
{
    [SerializeField] private PoopOutputController poopOutputController;
    [SerializeField] private Image gaugeImage;

    private void Start()
    {
        poopOutputController.InputValue
            .Subscribe(value =>
            {
                Debug.Log(value);
                gaugeImage.fillAmount += value / 100;
            });
    }
}
