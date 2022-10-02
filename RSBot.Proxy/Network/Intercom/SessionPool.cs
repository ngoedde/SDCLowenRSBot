using System.Collections.Concurrent;

namespace RSBot.Proxy.Network.Intercom;

internal class SessionPool : ConcurrentDictionary<Guid, Session>
{
}