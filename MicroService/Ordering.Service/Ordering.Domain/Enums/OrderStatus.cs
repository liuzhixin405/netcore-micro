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
        Pending=2,        // 
        [Description("付款完成")]
        Processing=3,     // 
        [Description("已发货")]
        Shipped=4,        // 
        [Description("已送达")]
        Delivered=5,      // 
        [Description("已完成")]
        Completed=6,      // 
        [Description("已取消")]
        Canceled=7,       // 
        [Description("失败")]
        Failed=8          // 
    }
}
