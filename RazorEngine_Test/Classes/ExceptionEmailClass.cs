using System;

namespace RazorEngine_Test
{
    public class ExceptionEmailClass
    {
        public Exception AppException { get; set; }
        public string ExceptionSource => AppException.Source;
        public string ErrorLineNumber => AppException.StackTrace == null
            ? string.Empty
            : AppException.StackTrace.Substring(AppException.StackTrace.IndexOf("line", StringComparison.Ordinal), 7);
        public string ErrorMessage => AppException.GetType().Name;
        public string ExceptionType => AppException.GetType().ToString();
        public string ErrorLocation => AppException.Message;
        public string LogDate => DateTime.Now.ToShortDateString();
        public string StackTrace => AppException.StackTrace;
        public string AppName { get; set; }
    }
}