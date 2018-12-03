using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasJsonValidator.Core.ObjectModel
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public ValidationErrors Errors { get; set; }
    }
}
