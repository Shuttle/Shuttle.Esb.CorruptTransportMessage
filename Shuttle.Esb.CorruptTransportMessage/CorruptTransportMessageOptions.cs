namespace Shuttle.Esb.CorruptTransportMessage
{
	public class CorruptTransportMessageOptions
	{
		public const string SectionName = "Shuttle:Modules:CorruptTransportMessage";

		public string MessageFolder { get; set; } = ".\\corrupt-transport-messages";
	}
}