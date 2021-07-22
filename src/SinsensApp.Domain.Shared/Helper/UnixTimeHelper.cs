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
        public static long DateTimeToUnixTime(DateTime dateTime)
        {
            DateTimeOffset dto = new DateTimeOffset(dateTime);
            return dto.ToUnixTimeSeconds();
        }

        /// <summary>
        /// 将Unix时间戳转换为dateTime格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime UnixTimeToDateTime(long time)
        {
            var dto = DateTimeOffset.FromUnixTimeSeconds(time);
            return dto.ToLocalTime().DateTime;
        }
    }
}