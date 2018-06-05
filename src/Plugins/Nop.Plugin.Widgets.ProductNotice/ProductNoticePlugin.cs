using Nop.Services.Cms;
using Nop.Core;
using Nop.Core.Plugins;
using System;
using System.Collections.Generic;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.ProductNotice
{
    /// <summary>
    /// Plugin for adding a notice to the top of the product details page in the public store
    /// </summary>
    public class ProductNoticePlugin : BasePlugin, IWidgetPlugin
    {
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public ProductNoticePlugin(ISettingService settingService, IWebHelper webHelper)
        {
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return _webHelper.GetStoreLocation() + "Admin/WidgetsProductNotice/Configure";
        }

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
           var settings = new ProductNoticeSettings()
           {
               Notice = ""
           };
            _settingService.SaveSetting(settings);

            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.ProductNotice.Notice", "Product Notice");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.ProductNotice.Notice.Hint", "This is a notice that will appear on top of the product details page in the public store.");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            _settingService.DeleteSetting<ProductNoticeSettings>();

            this.DeletePluginLocaleResource("Plugins.Widgets.ProductNotice.Notice");
            this.DeletePluginLocaleResource("Plugins.Widgets.ProductNotice.Notice.Hint");

            base.Uninstall();
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string> { "productdetails_top",  };
        }

        /// <summary>
        /// Gets a view component for displaying plugin in public store
        /// </summary>
        /// <param name="widgetZone">Name of the widget zone</param>
        /// <param name="viewComponentName">View component name</param>
        public void GetPublicViewComponent(string widgetZone, out string viewComponentName)
        {
            viewComponentName = "WidgetsProductNotice";
        }
    }
}
