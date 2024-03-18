using UnityEngine;

namespace Floor
{
    public class FloorController : MonoBehaviour
    {
        [SerializeField] private Floor _floorPrefab;
        [SerializeField] private Vector3 _startPosition;

        private Floor _firstFloor;
        private Floor _secondFloor;

        private void Awake()
        {
            _firstFloor = Instantiate(_floorPrefab, _startPosition, Quaternion.identity, transform);
            _firstFloor.PlayerPassedCurrentFloor += OnPlayerPassedCurrentFloor;

            var nextFloorPosition = GetNextFloorPosition(_firstFloor);

            _secondFloor = Instantiate(_floorPrefab, nextFloorPosition, Quaternion.identity, transform);
            _secondFloor.PlayerPassedCurrentFloor += OnPlayerPassedCurrentFloor;

        }

        private Vector3 GetNextFloorPosition(Floor currentFloor)
        {
            var position = currentFloor.transform.position;
            var currentFloorSize = currentFloor.GetSize();
            var nextFloorPositionX = currentFloorSize.x + position.x;

            return new Vector3(nextFloorPositionX, position.y, position.z);
        }

        private void OnPlayerPassedCurrentFloor(Floor currentFloor)
        {
            var nextFloorPosition = GetNextFloorPosition(currentFloor);

            if (_firstFloor == currentFloor)
            {
                _secondFloor.transform.position = nextFloorPosition;
            }
            else
            {
                _firstFloor.transform.position = nextFloorPosition;
            }
        }

        private void OnDestroy()
        {
            _firstFloor.PlayerPassedCurrentFloor -= OnPlayerPassedCurrentFloor;
            _secondFloor.PlayerPassedCurrentFloor -= OnPlayerPassedCurrentFloor;
        }
    }
}