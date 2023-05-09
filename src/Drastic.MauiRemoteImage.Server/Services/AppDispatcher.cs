using Drastic.Services;

namespace Drastic.MauiRemoteImage.Server.Services;

public class AppDispatcher : IAppDispatcher
{
    public bool Dispatch(Action action)
    {
        action.Invoke();
        return true;
    }
}