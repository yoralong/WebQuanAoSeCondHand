using System.Web;
using System.Web.Mvc;

namespace Web_Bán_Quần_Áo_SecondHand
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
