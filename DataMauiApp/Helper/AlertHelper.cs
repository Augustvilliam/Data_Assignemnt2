using Microsoft.Maui.Controls;
using System.Linq;
using System.Threading.Tasks;

namespace DataMauiApp.Helpers
{
    public static class AlertHelper
    {
        public static async Task ShowSelectionAlert(string entityType)
        {
            var currentPage = GetCurrentPage();
            if (currentPage != null)
            {
                await currentPage.DisplayAlert(
                    "Selection Required",
                    $"Please select a {entityType} before proceeding.",
                    "OK");
            }
        }

        private static Page? GetCurrentPage()
        {
            return Application.Current?.MainPage?.Navigation?.NavigationStack.LastOrDefault();
        }
    }
}
