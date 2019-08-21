using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using wxapi.Controllers;
using wxapi.Data;
using wxapi.Helpers;
using wxapi.Services;
using Xunit;

namespace wxapi.tests.Controllers
{
	public class ProductControllerTests
	{
		[Theory]
		[InlineData(SortOption.Low, typeof(PriceLow2HighSorter))]
		[InlineData(SortOption.High, typeof(PriceHigh2LowSorter))]
		[InlineData(SortOption.Ascending, typeof(NameA2ZSorter))]
		[InlineData(SortOption.Descending, typeof(NameZ2ASorter))]
		[InlineData(SortOption.Recommended, typeof(PopularityHigh2LowSorter))]
		[InlineData(SortOption.Unknown, null)]
		public void GetSorter(SortOption sortOption, Type expectedSorterType)
		{
			var productServiceMock = new Mock<IProductService>();
			var shopperHistoryServiceMock = new Mock<IShopperHistoryService>();
			var productController = new ProductController(productServiceMock.Object, shopperHistoryServiceMock.Object);

			var actual = productController.GetSorter(sortOption);

			Assert.IsType(expectedSorterType, actual);
		}

		[Fact]
		public async Task Sort()
		{
			var product1 = new Product { Name = "A", Price = 1.0, Quantity = 100 };
			var product2 = new Product { Name = "Z", Price = 99.0, Quantity = 200 };
			var list = new[] { product1, product2 } as IEnumerable<Product>;
			var productServiceMock = new Mock<IProductService>();
			var shopperHistoryServiceMock = new Mock<IShopperHistoryService>();
			productServiceMock.Setup(m => m.GetProducts()).Returns(Task.FromResult(list));

			var productController = new ProductController(productServiceMock.Object, shopperHistoryServiceMock.Object);
			var result = await productController.Sort(SortOption.Ascending);
			var okResult = result as OkObjectResult;

			Assert.NotNull(okResult);

			var actual = okResult.Value;

			Assert.NotNull(actual);
		}
	}
}
