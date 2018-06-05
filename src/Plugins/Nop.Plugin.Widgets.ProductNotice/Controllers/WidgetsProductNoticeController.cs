using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.ProductNotice.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.ProductNotice.Controllers
{
    [Area(AreaNames.Admin)]
    public class WidgetsProductNoticeController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreService _storeService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;

        public WidgetsProductNoticeController(
            IWorkContext workContext,
            IStoreService storeService, 
            IPermissionService permissionService,
            ISettingService settingService,
            ILocalizationService localizationService)
        {
            this._workContext = workContext;
            this._storeService = storeService;
            this._permissionService = permissionService;
            this._settingService = settingService;
            this._localizationService = localizationService;
        }

        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var productNoticeSettings = _settingService.LoadSetting<ProductNoticeSettings>(storeScope);

            var model = new ConfigurationModel()
            {
                ActiveStoreScopeConfiguration = storeScope,
                Notice = productNoticeSettings.Notice
            };

            if (storeScope > 0)
            {
                model.Notice_OverrideForStore = _settingService.SettingExists(productNoticeSettings, x => x.Notice, storeScope);
            }

            return View("~/Plugins/Widgets.ProductNotice/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageWidgets))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var productNoticeSettings = _settingService.LoadSetting<ProductNoticeSettings>(storeScope);

            productNoticeSettings.Notice = model.Notice;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(productNoticeSettings, x => x.Notice, model.Notice_OverrideForStore, storeScope, false);

            //now clear settings cache
            _settingService.ClearCache();

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }
    }
}
