using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Enums
{
    public enum OrderStatus
    {
        Pending,        // 待处理
        Processing,     // 处理中
        Shipped,        // 已发货
        Delivered,      // 已送达
        Completed,      // 已完成
        Canceled,       // 已取消
        Failed          // 失败
    }
}
