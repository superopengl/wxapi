using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wxapi.Data;
using wxapi.Helpers;
using wxapi.Services;

[assembly: InternalsVisibleTo("wxapi.specs")]
namespace wxapi.Controllers
{
	[Route("api/[controller]")]
	public class ProductController : ControllerBase
	{
		private readonly IProductService productService;
		private readonly IShopperHistoryService shopperHistoryService;

		public ProductController(IProductService productService, IShopperHistoryService shopperHistoryService)
		{
			this.productService = productService;
			this.shopperHistoryService = shopperHistoryService;
		}

		[HttpGet("sort")]
		public async Task<IActionResult> Sort([FromQuery(Name = "sortOption")] SortOption sortOption)
		{
			if(sortOption == SortOption.Unknown)
			{
				return BadRequest("SortOption not supported");
			}
			var sorter = GetSorter(sortOption);
			if(sorter == null)
			{
				return BadRequest("SortOption is invalid");
			}

			var productTask = productService.GetProducts();
			var result = await sorter.Sort(productTask);

			return Ok(result);
		}

		internal IProductSorter GetSorter(SortOption sortOption)
		{
			switch (sortOption)
			{
				case SortOption.High:
					return new PriceHigh2LowSorter();
				case SortOption.Low:
					return new PriceLow2HighSorter();
				case SortOption.Ascending:
					return new NameA2ZSorter();
				case SortOption.Descending:
					return new NameZ2ASorter();
				case SortOption.Recommended:
					return new PopularityHigh2LowSorter(shopperHistoryService);
				case SortOption.Unknown:
				default:
					return null;
			}
		}
	}

	public enum SortOption
	{
		Unknown = 0,
		Low,
		High,
		Ascending,
		Descending,
		Recommended
	}
}
