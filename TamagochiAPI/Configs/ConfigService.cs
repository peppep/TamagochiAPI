using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;

namespace TamagochiAPI.Configs
{
	internal enum ConfigKeys
	{
		HappinessDecreaseStepSec,
		HungryDecreaseStepSec,
		SessionTimeSec,
		DefaultHungryLevel,
		DefaultHappinessLevel
	}

	public interface IConfigService
	{
		T GetConfigValue<T>(string key1);
		T GetConfigValue<T>(string key1, string key2);
	}

	public class ConfigService : IConfigService
	{
		private static string ConfigPath = HttpRuntime.AppDomainAppPath + "Configs\\configuration.json";

		private readonly JObject parsedConfig;

		public ConfigService()
		{
			var fileContent = File.ReadAllText(ConfigPath);
			parsedConfig = JObject.Parse(fileContent);
		}

		public T GetConfigValue<T>(string key1)
		{
			var res = parsedConfig[key1];

			return JsonConvert.DeserializeObject<T>(res.ToString());
		}

		public T GetConfigValue<T>(string key1, string key2)
		{
			var res = parsedConfig[key1][key2];

			return JsonConvert.DeserializeObject<T>(res.ToString());
		}
	}
}