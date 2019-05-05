using System;
using TechTalk.SpecFlow;

namespace API.GherkinSpec
{
    [Binding]
    public class APISteps
    {
        [Given(@"The server is onlines")]
        public void GivenTheServerIsOnlines()
        {
            ScenarioContext.Current.Pending();
        }
    }
}
