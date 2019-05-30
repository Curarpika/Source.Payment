using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Source.Auth.Models
{
    // Add profile data for application users by adding properties to the ESHUser class
    public class BaseUser : IdentityUser<Guid>
    {
        /// <summary>
        /// 名
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string LastName { get; set; }

        public decimal Credit { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        // alipay = 0, wechatpay = 1
        public int ExternalType { get; set; }
        public string ExternalId { get; set; }
        public string ExternalName { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return LastName + FirstName; }
        }
    }
}
