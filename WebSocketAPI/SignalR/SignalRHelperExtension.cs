using Microsoft.AspNetCore.SignalR;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace WebSocketAPI.SignalR
{
    public static class SignalRHelperExtension
    {
        public static IEnumerable<string> ToList(this IGroupManager groupManager)
        {
            object lifetimeManager = groupManager.GetType().GetRuntimeFields()
                .Single(fi => fi.Name == "_lifetimeManager")
                .GetValue(groupManager)!;

            object groupsObject = lifetimeManager.GetType().GetRuntimeFields()
                .Single(fi => fi.Name == "_groups")
                .GetValue(lifetimeManager)!;

            IDictionary? groupsDictionary = groupsObject.GetType().GetRuntimeFields()
                .Single(fi => fi.Name == "_groups")
                .GetValue(groupsObject) as IDictionary;

            return groupsDictionary!.Keys.Cast<string>().ToList();
        }
    }
}
