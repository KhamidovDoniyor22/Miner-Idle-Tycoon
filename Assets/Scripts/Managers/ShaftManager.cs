using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaftManager : Singleton<ShaftManager>
{
    [SerializeField] private Shaft shaftPrefab;
    [SerializeField] private float newShaftYPosition;
    [SerializeField] private int newShaftCost = 500;

    [Header("Shaft")]
    [SerializeField] private List<Shaft> shafts;

    public List<Shaft> Shafts => shafts;
    private ShaftUI _shaftUI;

    private int _currentShaftIndex;

    public int NewShaftCost => newShaftCost;

    private void Awake()
    {
        _currentShaftIndex = PlayerPrefs.GetInt("_currentShaftIndex", _currentShaftIndex);
        newShaftCost = PlayerPrefs.GetInt("newShaftCost", newShaftCost);
    }

    private void Start()
    {
        shafts[0].ShaftID = 0;
        ShaftCountSave();

        _shaftUI = FindObjectOfType<ShaftUI>();
        _shaftUI._priceText.text = $"{newShaftCost}";

    }
    public void AddShaft()
    {
        Transform lastShaft = shafts[_currentShaftIndex].transform;
        Shaft newShaft = Instantiate(shaftPrefab, lastShaft.position, Quaternion.identity);
        newShaft.transform.localPosition = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition, lastShaft.position.z);
        _shaftUI = FindObjectOfType<ShaftUI>();
        _shaftUI._priceText.text = $"{newShaftCost * 2}";
        _currentShaftIndex++;

        newShaft.ShaftID = _currentShaftIndex;
        shafts.Add(newShaft);
        
        newShaftCost *= 2;
        _shaftUI = lastShaft.GetComponent<ShaftUI>();
        _shaftUI._priceText.text = $"{newShaftCost}";

        PlayerPrefs.SetInt("_currentShaftIndex", _currentShaftIndex);
        PlayerPrefs.SetInt("newShaftCost", newShaftCost);
    }
    private void ShaftCountSave()
    {
        for(int i = 0; i < _currentShaftIndex; i++)
        {
            Transform lastShaft = shafts[i].transform;
            Shaft newShaft = Instantiate(shaftPrefab, lastShaft.position, Quaternion.identity);
            newShaft.transform.localPosition = new Vector3(lastShaft.position.x, lastShaft.position.y - newShaftYPosition, lastShaft.position.z);

            newShaft.ShaftID = i;
            shafts.Add(newShaft);

            _shaftUI = lastShaft.GetComponent<ShaftUI>();
            _shaftUI.buyNewShaftButton.SetActive(false);
            _shaftUI._priceText.text = $"{newShaftCost}";
        }
    }
}
