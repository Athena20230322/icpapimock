using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.AOP
{
    public class EnableAspectAttribute : ContextAttribute, IContributeObjectSink
    {
        public EnableAspectAttribute() : base(typeof(EnableAspectAttribute).FullName) { }

        public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink nextSink)
        {
            return new AspectTrace(nextSink);
        }
    }
}
