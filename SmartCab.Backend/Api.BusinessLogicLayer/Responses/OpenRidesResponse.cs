using System.Collections.Generic;
using Api.BusinessLogicLayer.DataTransferObjects;

namespace Api.BusinessLogicLayer.Responses
{
    public class OpenRidesResponse
    {
        public List<SoloRideDto> OpenSoloRides { get; set; }
        public List<List<SoloRideDto>> OpenSharedRides { get; set; }
    }
}