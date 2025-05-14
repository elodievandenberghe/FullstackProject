using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BookingSite.Helpers
{
    public static class ViewDataHelper
    {
        public static void SetPageTitle(this ViewDataDictionary viewData, string title, string icon = "airplane", string primaryAction = null)
        {
            viewData["Title"] = title;
            viewData["TitleIcon"] = icon;
            if (!string.IsNullOrEmpty(primaryAction))
            {
                viewData["PrimaryAction"] = primaryAction;
            }
        }
    }
}
