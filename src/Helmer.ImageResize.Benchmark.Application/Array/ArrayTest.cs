using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helmer.ImageResize.Benchmark.Application.Array
{
    internal static class ArrayTest
    {

		public static string TestWithArray()
		{
			
			var selectionItems = new List<SelectionItem> { new SelectionItem { HeaderId = 1 }, new SelectionItem { HeaderId = 2 } };
			var selectedProperties = new List<SelectedProperty> { new SelectedProperty { HeaderId = 1, DisplayValue = "One" }, new SelectedProperty { HeaderId = 2, DisplayValue = "Two" } };

			var selectedDisplayValues = selectionItems
				.Select(asi => selectedProperties.FirstOrDefault(sap => sap.HeaderId == asi.HeaderId)?.DisplayValue)
				.Where(s => !string.IsNullOrWhiteSpace(s))
				.ToArray();

			var displayValue = string.Join(", ", selectedDisplayValues);

			return displayValue;
        }

		public static string TestWithoutArray()
		{

			var selectionItems = new List<SelectionItem> { new SelectionItem { HeaderId = 1 }, new SelectionItem { HeaderId = 2 } };
			var selectedProperties = new List<SelectedProperty> { new SelectedProperty { HeaderId = 1, DisplayValue = "One" }, new SelectedProperty { HeaderId = 2, DisplayValue = "Two" } };

			var selectedDisplayValues = selectionItems
				.Select(asi => selectedProperties.FirstOrDefault(sap => sap.HeaderId == asi.HeaderId)?.DisplayValue)
				.Where(s => !string.IsNullOrWhiteSpace(s));

			var displayValue = string.Join(", ", selectedDisplayValues);

			return displayValue;
		}


    }

	internal class SelectionItem
	{
        public int HeaderId { get; set; }
    }

	internal class SelectedProperty
	{
		public int HeaderId { get; set; }

		public string DisplayValue { get; set; }
	}
}
