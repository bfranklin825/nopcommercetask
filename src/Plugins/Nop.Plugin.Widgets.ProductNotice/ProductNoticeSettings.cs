using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.ProductNotice
{
    public class ProductNoticeSettings : ISettings
    {
        public string Notice { get; set; }
    }
}
