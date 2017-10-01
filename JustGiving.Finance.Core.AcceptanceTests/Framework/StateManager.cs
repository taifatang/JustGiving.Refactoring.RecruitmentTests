using TechTalk.SpecFlow;

namespace JustGiving.Finance.Core.AcceptanceTests.Framework
{
    internal static class StateManager
    {
        public static T Get<T>(string key) where T : class
        {
            return ScenarioContext.Current[key] as T;
        }

        public static void Set(string key, object value)
        {
            ScenarioContext.Current[key] = value;
        }
    }
}
