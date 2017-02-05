using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web;

namespace TamagochiAPI.Configs
{
	public enum ConfigKeys
	{
		HappinessStep,
		HungerStep,
		SessionTimeSec,
		DefaultHungryLevel,
		DefaultHappinessLevel,
		StatsMinLevel,
		StatsMaxLevel
	}

	public interface IConfigService
	{
		T GetConfigValue<T>(ConfigKeys key1);
		T GetConfigValue<T>(ConfigKeys key1, object key2);
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

		public T GetConfigValue<T>(ConfigKeys key1)
		{
			var res = parsedConfig[key1.ToString()];

			return JsonConvert.DeserializeObject<T>(res.ToString());
		}

		public T GetConfigValue<T>(ConfigKeys key1, object key2)
		{
			var k1 = key1.ToString();
			var k2 = key2.ToString();
			var res = parsedConfig[k1][k2];

			return JsonConvert.DeserializeObject<T>(res.ToString());
		}
	}
}