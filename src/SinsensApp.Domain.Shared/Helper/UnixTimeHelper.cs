using System;
using System.Collections.Generic;
using System.Text;

namespace SinsensApp.Helper
{
    public static class UnixTimeHelper
    {
        /// <summary>
        /// 将dateTime格式转换为Unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimeMilliseconds(DateTime dateTime)
        {
            DateTimeOffset dto = new DateTimeOffset(dateTime);
            return dto.ToUnixTimeMilliseconds();
        }

        /// <summary>
        /// 将Unix时间戳转换为dateTime格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime FromUnixTimeMilliseconds(long time)
        {
            var dto = DateTimeOffset.FromUnixTimeMilliseconds(time);
            return dto.ToLocalTime().DateTime;
        }
    }
}