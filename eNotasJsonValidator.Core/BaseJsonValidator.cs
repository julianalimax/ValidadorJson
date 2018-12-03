using eNotasJsonValidator.Core.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNotasJsonValidator.Core
{
    public abstract class BaseJsonValidator
    {
        public abstract ValidationResult Validate(string json);
    }
}
