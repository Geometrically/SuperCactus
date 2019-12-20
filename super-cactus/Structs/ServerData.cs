using System.Collections.Generic;
using Discord;

namespace super_cactus.Structs
{
    public struct ServerData
    {
        public ulong CalendarChannelId;
        public ulong ModGroupId;

        public List<Event> Events;

        public ulong Id;
    }
}