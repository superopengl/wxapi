using System.Collections.Generic;
using System.Runtime.Serialization;

namespace wxapi.Data
{
	[DataContract]
	public class ShopperHistory
	{
		[DataMember(Name = "customerId")]
		public long CustomerId { get; set; }
		[DataMember(Name = "products")]
		public List<Product> Products { get; set; }
	}
}
