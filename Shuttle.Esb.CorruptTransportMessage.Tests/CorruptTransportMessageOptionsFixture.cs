using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Shuttle.Esb.CorruptTransportMessage.Tests
{
	[TestFixture]
	public class CorruptTransportMessageOptionsFixture
	{
		protected CorruptTransportMessageOptions GetOptions()
		{
			var result = new CorruptTransportMessageOptions();

			new ConfigurationBuilder()
				.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\appsettings.json")).Build()
				.GetRequiredSection($"{CorruptTransportMessageOptions.SectionName}").Bind(result);

			return result;
		}

		[Test]
		public void Should_be_able_to_load_the_configuration()
		{
			var options = GetOptions();

			Assert.IsNotNull(options);
			Assert.AreEqual(".\\folder", options.MessageFolder);
		}
	}
}