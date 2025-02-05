using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] GameObject BoxUIPrefab;

    private bool _isPlayerInRange = false;

    public int BoxIndex { get; set; }

    public void IsPlayerInRange(bool isPlayerInRange)
    {
        _isPlayerInRange = isPlayerInRange;
    }

}
