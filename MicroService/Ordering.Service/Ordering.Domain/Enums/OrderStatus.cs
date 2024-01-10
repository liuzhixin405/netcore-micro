using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Enums
{
    public enum OrderStatus
    {
        [Description("待确认")]
        BeConfirm=1,
        [Description("待付款")]
        Pending,        // 
        [Description("付款完成")]
        Processing,     // 
        [Description("已发货")]
        Shipped,        // 
        [Description("已送达")]
        Delivered,      // 
        [Description("已完成")]
        Completed,      // 
        [Description("已取消")]
        Canceled,       // 
        [Description("失败")]
        Failed          // 
    }
}
