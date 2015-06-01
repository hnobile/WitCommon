using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourcesGenerator
{
    class ResourcesImporterException : Exception
    {
        public enum ResourceImporterExceptionType
        {
            InvalidType,
            InvalidResources,
            InvalidDescription,
            InvalidDeadLine,
            InvalidDuration,
            InvalidQuantityOfQuestions,
            InvalidMinimumCalification,
            InvalidMaximumCalification,
            InvalidCalificationToApprove,
            InvalidOrder,
            InvalidQuestions,
        }

        public List<ResourceImporterExceptionType> ErrorTypes = new List<ResourceImporterExceptionType>();

        public ResourcesImporterException()
            : base()
        {
        }
        public ResourcesImporterException(string message)
            : base(message)
        {
        }
        public ResourcesImporterException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
