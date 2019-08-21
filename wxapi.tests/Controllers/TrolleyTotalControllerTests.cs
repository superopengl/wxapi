using System;
using System.Collections.Generic;
using wxapi.Controllers;
using Xunit;
using static wxapi.Controllers.TrolleyTotalController;

namespace wxapi.tests.Controllers
{
	public class TrolleyTotalControllerTests
	{
		public TrolleyTotalControllerTests()
		{
		}

		[Fact]
		public void UseSpecialDynamicProgramming()
		{
			var priceDic = new Dictionary<string, double> {
				{ "Apple", 5.0 },
				{ "Banana", 7.0 },
				{ "Cola", 3.0 },
				{ "Orange", 11.0 },
			};
			var quantity = new Dictionary<string, int> {
				{ "Apple", 2},
				{ "Banana", 3 }
			};
			var specials = new[] {
				new TrolleyCalculatorSpecial
				{
					Total = 5.0,
					Quantities = new[] {
						new TrolleyCalculatorQuantity("Apple",2),
						new TrolleyCalculatorQuantity("Cola", 3)
					}
				},
				new TrolleyCalculatorSpecial
				{
					Total = 5.0,
					Quantities = new[] {
						new TrolleyCalculatorQuantity("Orange",2),
						new TrolleyCalculatorQuantity("Banana", 3)
					}
				},
			};

			var sub = new TrolleyTotalController();
			var total = sub.UseSpecial(quantity, priceDic, specials, 0, 0);

			Assert.Equal((decimal)10.0, total);
		}

		[Fact]
		public void UseSpecialDynamicProgramming2()
		{
			var priceDic = new Dictionary<string, double> {
				{ "Apple", 1.0 },
				{ "Banana", 1.1 },
				{ "Cola", 3.0 },
				{ "Orange", 11.0 },
			};
			var quantity = new Dictionary<string, int> {
				{ "Apple", 2},
				{ "Banana", 3 }
			};
			var specials = new[] {
				new TrolleyCalculatorSpecial
				{
					Total = 5.0,
					Quantities = new[] {
						new TrolleyCalculatorQuantity("Apple",2),
						new TrolleyCalculatorQuantity("Cola", 3)
					}
				},
				new TrolleyCalculatorSpecial
				{
					Total = 5.0,
					Quantities = new[] {
						new TrolleyCalculatorQuantity("Orange",2),
						new TrolleyCalculatorQuantity("Banana", 3)
					}
				},
			};

			var sub = new TrolleyTotalController();
			var total = sub.UseSpecial(quantity, priceDic, specials, 0, 0);

			Assert.Equal((decimal)5.3, total);
		}
	}
}
