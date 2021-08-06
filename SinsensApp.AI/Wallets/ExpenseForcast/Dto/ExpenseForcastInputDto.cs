using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SinsensApp.AI.Event.Eto
{
    public class ExpenseForcastInputDto
    {
        [Required]
        public string Day { get; set; }

        public Guid? TenantId { get; set; }

        [Required]
        public Guid? UserId { get; set; }
    }
}