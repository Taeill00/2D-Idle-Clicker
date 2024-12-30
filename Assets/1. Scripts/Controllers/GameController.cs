using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textCurGems;
    [SerializeField] private Data data;

    private void Awake()
    {
        data = new Data();
        data.curGems = 1;
    }

    private void Update()
    {
        textCurGems.text = $"{data.curGems} Flasks";
    }

    public void OnBtnGenerateFlasks()
    {
        data.curGems += 1;
    }
}
