using System.Collections;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private TileState _state;
    private Renderer _renderer;
    [SerializeField] private TMP_Text _text;
    public int number { get; set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }
    public void ChangeState(TileState state)
    {
        _state = state;
        ChangeValueText(state.number);
    }
    public void ChangeValueText(int value)
    {
        number = value;
        _text.text = value.ToString();
    }
    public IEnumerator ChangeLocalPosition(Vector2 targetPosition)
    {
        Vector2 startPosition = transform.localPosition;
        transform.localRotation = Quaternion.identity;

        float duration = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition; // Đảm bảo vị trí cuối cùng chính xác
    }


}
