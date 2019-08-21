using System.Runtime.Serialization;

namespace wxapi.Data
{
	[DataContract]
	public class Product
    {
		[DataMember(Name = "name")]
		public string Name { get; set; }
		[DataMember(Name = "price")]
		public double Price { get; set; }
		[DataMember(Name = "quantity")]
		public int Quantity { get; set; }

	}
}