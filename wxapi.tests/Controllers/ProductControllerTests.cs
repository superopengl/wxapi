using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Linq;
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

			if (expectedSorterType == null)
			{
				Assert.Null(actual);
			}
			else
			{
				Assert.IsType(expectedSorterType, actual);
			}
		}

		private async Task<List<Product>> SortBy(SortOption sortOption, IEnumerable<Product> seeds)
		{
			var productServiceMock = new Mock<IProductService>();
			var shopperHistoryServiceMock = new Mock<IShopperHistoryService>();
			productServiceMock.Setup(m => m.GetProducts()).Returns(Task.FromResult(seeds));

			var productController = new ProductController(productServiceMock.Object, shopperHistoryServiceMock.Object);
			var result = await productController.Sort(sortOption);
			var okResult = result as OkObjectResult;

			var list = okResult?.Value as IEnumerable<Product>;
			return list?.ToList();
		}

		[Fact]
		public async Task Sort_Low()
		{
			var product1 = new Product { Name = "K", Price = 1.0, Quantity = 100 };
			var product2 = new Product { Name = "Z", Price = 99.0, Quantity = 200 };
			var product3 = new Product { Name = "A", Price = 30.0, Quantity = 200 };
			var list = new[] { product1, product2, product3 } as IEnumerable<Product>;

			var actual = await SortBy(SortOption.Low, list);

			Assert.NotNull(actual);
			Assert.Equal(product1, actual[0]);
			Assert.Equal(product3, actual[1]);
			Assert.Equal(product2, actual[2]);
		}

		[Fact]
		public async Task Sort_High()
		{
			var product1 = new Product { Name = "K", Price = 1.0, Quantity = 100 };
			var product2 = new Product { Name = "Z", Price = 99.0, Quantity = 200 };
			var product3 = new Product { Name = "A", Price = 30.0, Quantity = 200 };
			var list = new[] { product1, product2, product3 } as IEnumerable<Product>;

			var actual = await SortBy(SortOption.High, list);

			Assert.NotNull(actual);
			Assert.Equal(product2, actual[0]);
			Assert.Equal(product3, actual[1]);
			Assert.Equal(product1, actual[2]);
		}

		[Fact]
		public async Task Sort_Ascending()
		{
			var product1 = new Product { Name = "K", Price = 1.0, Quantity = 100 };
			var product2 = new Product { Name = "Z", Price = 99.0, Quantity = 200 };
			var product3 = new Product { Name = "A", Price = 30.0, Quantity = 200 };
			var list = new[] { product1, product2, product3 } as IEnumerable<Product>;

			var actual = await SortBy(SortOption.Ascending, list);

			Assert.NotNull(actual);
			Assert.Equal(product3, actual[0]);
			Assert.Equal(product1, actual[1]);
			Assert.Equal(product2, actual[2]);
		}

		[Fact]
		public async Task Sort_Descending()
		{
			var product1 = new Product { Name = "K", Price = 1.0, Quantity = 100 };
			var product2 = new Product { Name = "Z", Price = 99.0, Quantity = 200 };
			var product3 = new Product { Name = "A", Price = 30.0, Quantity = 200 };
			var list = new[] { product1, product2, product3 } as IEnumerable<Product>;

			var actual = await SortBy(SortOption.Descending, list);

			Assert.NotNull(actual);
			Assert.Equal(product2, actual[0]);
			Assert.Equal(product1, actual[1]);
			Assert.Equal(product3, actual[2]);
		}
	}
}
