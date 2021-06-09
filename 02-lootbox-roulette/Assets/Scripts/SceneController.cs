using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private const float SpeedCoefficient = 0.001f;
    private const int ItemsAmount = 20;

    private Unity.Mathematics.Random _random;
    private float _rollingDistance = 15;
    private float _itemWidth;
    private float _gapWidth;
    private float _nextPosition = -11;
    private float _rollingElapsed;
    private int _itemsToScrollAmount;
    private List<GameObject> _items;
    private Color[] _rarities;

    public GameObject item;

    // Start is called before the first frame update
    void Start()
    {
        _rarities = new[] {new Color(0, 0, 255), new Color(255, 255, 0), new Color(204, 0, 204)};
        _random = new Unity.Mathematics.Random(10);
        _itemWidth = item.GetComponent<SpriteRenderer>().size.x;
        _gapWidth = _itemWidth / 10;
        _itemsToScrollAmount = _random.NextInt(10, 20);
        _rollingDistance = _itemsToScrollAmount * (_itemWidth + _gapWidth);
        _items = Enumerable.Range(0, ItemsAmount)
            .Select(InstantiateItem)
            .ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rollingElapsed < _rollingDistance)
        {
            _items.ForEach(MoveItem);
            _rollingElapsed += Time.deltaTime;
        }
    }

    private GameObject InstantiateItem(int i)
    {
        _nextPosition += _itemWidth + _gapWidth;
        var it = Instantiate(item, new Vector3(_nextPosition, 0, 0), Quaternion.identity);
        it.GetComponent<SpriteRenderer>().color = GetRandomRarity();
        return it;
    }

    private Color GetRandomRarity()
    {
        var i = _random.NextInt(100);
        if (i < 5)
        {
            return _rarities[2];
        }

        if (i < 20)
        {
            return _rarities[1];
        }

        return _rarities[0];
    }

    private void MoveItem(GameObject it)
    {
        if (it.transform.position.x < -11)
        {
            it.transform.position = new Vector3(11, it.transform.position.y);
            it.GetComponent<SpriteRenderer>().color = GetRandomRarity();
        }

        it.transform.position += Vector3.left * ((_rollingDistance - _rollingElapsed) * SpeedCoefficient);
    }
}