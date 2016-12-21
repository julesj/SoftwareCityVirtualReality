using UnityEngine;
using System.Collections;

public class Events {

	public class BuildingSelectedEvent
    {
        public Building building;
        public BuildingSelectedEvent(Building building)
        {
            this.building = building;
        }
    }

    public class BuildingSelectionConfirmedEvent
    {

    }

    public class ClearDisplayEvent
    {

    }

    public class ResetPlayerEvent
    {

    }
}
