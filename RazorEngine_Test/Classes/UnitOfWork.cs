using System;

namespace RazorEngine_Test
{
    public class UnitOfWork 
    {
        #region Ctor
        public UnitOfWork()
        { }
        #endregion

        public void SendTestErrorEmail()
        {
            try
            {
                TemplatedErrorEmail.SendExceptionEmail(new ExceptionEmailClass
                {
                    AppException =
                        new ApplicationException(
                            @"Application threw an invalid program exception and will be terminated.")
                });
            }
            catch (Exception e)
            {
                Logger.Error($"Error Details: {e.Message}\r\nStack: {e.StackTrace}");
            }
        }
    }
}
