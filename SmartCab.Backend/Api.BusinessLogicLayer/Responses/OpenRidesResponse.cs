using System;
using System.Collections.Generic;
using Api.BusinessLogicLayer.DataTransferObjects;
using Api.DataAccessLayer.Models;

namespace Api.BusinessLogicLayer.Responses
{
    public class OpenRidesResponse
    {
        public List<SoloRideDto> OpenSoloRides { get; set; }
        public List<List<SoloRideDto>> OpenSharedRides { get; set; }
    }
}