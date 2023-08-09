using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bhaki.API.Enums
{
    public enum AccountStatus
    {
        [Display(Name= "#4ccb19, Green")]
        Paid,
        [Display(Name = "#B11a03, Red")]
        Outstanding,
        [Display(Name = "#Cb4419, Orange")]
        Partial
    }

}
