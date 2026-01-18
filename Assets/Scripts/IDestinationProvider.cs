using UnityEngine;
using System.Collections.Generic;

public interface IDestinationProvider
{
    string GetDestinationRoomId(string currentRoomId, Direction exitDirection);
}
