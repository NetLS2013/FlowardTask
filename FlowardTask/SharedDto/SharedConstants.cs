using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedDto
{
    public static class SharedConstants
    {
        public const string RABBIT_MQ_HOST = "localhost";
        public const string RABBIT_MQ_EMAIL_QUEUE = "EmailsQueue";
        public const bool RABBIT_MQ_DURABLE = false;
        public const bool RABBIT_MQ_EXCLUSIVE = false;
        public const bool RABBIT_MQ_AUTODELETE = false;
        public const string RABBIT_MQ_EXCHANGE = "demo.exchange";
        public const string RABBIT_MQ_ROUTING_KEY = "EmailsQueue.*";
        public const IDictionary<string, object> RABBIT_MQ_ARGUMENTS = null;
        public const uint RABBIT_MQ_PREFETCHSIZE = 0;
        public const ushort RABBIT_MQ_PREFETCHCOUNT = 1;
        public const bool RABBIT_MQ_GLOBAL = false;
        public const bool RABBIT_MQ_AUTOACKL = false;
    }
}
