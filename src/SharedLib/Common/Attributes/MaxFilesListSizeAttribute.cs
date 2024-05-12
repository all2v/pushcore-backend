using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Astrum.SharedLib.Common.Attributes
{
    public class MaxFilesListSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFilesSize;
        public MaxFilesListSizeAttribute(int maxFilesSize)
        {
            _maxFilesSize = maxFilesSize;
        }

        protected override ValidationResult IsValid(
        object value, ValidationContext validationContext)
        {
            var files = value as List<IFormFile>;
            if (files != null)
            {
                var totalSize = files.Sum(f => f.Length);
                if (totalSize > _maxFilesSize)
                {
                    return new ValidationResult(GetErrorMessage(totalSize));
                }
            }

            return ValidationResult.Success;
        }

        public string GetErrorMessage(long totalSize)
        {
            return $"Maximum allowed files size is {_maxFilesSize} bytes, yours - {totalSize}.";
        }
    }
}
