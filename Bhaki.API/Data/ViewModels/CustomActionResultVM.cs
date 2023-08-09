using Bhaki.API.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bhaki.API.Data.ViewModels
{
    public class CustomActionResultVM
    {
        public Exception Exception { get; set; }
        public Student student { get; set; }
    }
}
