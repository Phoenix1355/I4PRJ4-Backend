using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Api.BusinessLogicLayer.Requests;
using Api.DataAccessLayer.UnitTests.Factories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NUnit.Framework;
using SmartCabPoc.Integration.Test;
using SmartCabPoc.Integration;
namespace Api.IntegrationTests.Customer
{
    [TestFixture]
    public class RidesTests : IntegrationSetup
    {

       
    }
}