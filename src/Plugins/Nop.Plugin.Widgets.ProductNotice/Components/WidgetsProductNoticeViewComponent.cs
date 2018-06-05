using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.ProductNotice.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.ProductNotice.Components
{
    [ViewComponent(Name = "WidgetsProductNotice")]
    public class WidgetsProductNoticeViewComponent : NopViewComponent
    {
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly IPictureService _pictureService;

        public WidgetsProductNoticeViewComponent(IStoreContext storeContext,
            IStaticCacheManager cacheManager,
            ISettingService settingService,
            IPictureService pictureService)
        {
            this._storeContext = storeContext;
            this._cacheManager = cacheManager;
            this._settingService = settingService;
            this._pictureService = pictureService;
        }

        public IViewComponentResult Invoke(string widgetZone, object additionalData)
        {
            var productNoticeSettings =
                _settingService.LoadSetting<ProductNoticeSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel
            {
                Notice = productNoticeSettings.Notice
            };

            return View("~/Plugins/Widgets.ProductNotice/Views/PublicInfo.cshtml", model);
        }
    }
}
