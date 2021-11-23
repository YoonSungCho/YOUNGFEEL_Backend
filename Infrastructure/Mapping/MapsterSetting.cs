using Mapster;

namespace Infrastructure.Mapping
{
	public static class MapsterSetting
	{
		public static void Configure()
		{
			TypeAdapterConfig.GlobalSettings.Default.IgnoreNullValues(true);
		}
	}
}