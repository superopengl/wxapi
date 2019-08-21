using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Moq;
using wxapi.Services;
using Xunit;

namespace wxapi.specs.Services
{
	public class RestClientTests
	{
		public RestClientTests()
		{
		}

		[Theory]
		[InlineData("http://www.123.com", "http://www.123.com/?token={0}")]
		[InlineData("http://www.123.com?xyz=123", "http://www.123.com/?xyz=123&token={0}")]
		[InlineData("http://www.123.com?token=123", "http://www.123.com/?token=123")]
		public void GetUrlWithToken(string input, string expectedFormat)
		{
			var fakeToken = Guid.NewGuid().ToString();
			var configServiceMock = new Mock<IConfigService>();
			configServiceMock.Setup(m => m.GetToken()).Returns(fakeToken);

			var restClient = new RestClient(configServiceMock.Object);

			var actual = restClient.GetUrlWithToken(input);
			var expected = string.Format(expectedFormat, fakeToken);

			Assert.Equal(expected, actual);
		}

		public async Task GetAsync()
		{

		}
	}
}
