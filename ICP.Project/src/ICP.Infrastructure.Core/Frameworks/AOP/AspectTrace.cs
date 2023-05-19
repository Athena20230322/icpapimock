using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Frameworks.AOP
{
    /// <summary>
    /// When a method call is made on the proxy, the remoting infrastructure provides the necessary
    /// support for passing the arguments to the actual object across the remoting boundaries,
    /// calling the actual object method with the arguments, and returning the results back to the
    /// client of the proxy object.
    /// </summary>
    internal sealed class AspectTrace : IMessageSink
    {
        /// <summary>
        /// Gets the next message sink in the sink chain.
        /// </summary>
        public IMessageSink NextSink { get; private set; }

        /// <summary>
        /// When a method call is made on the proxy, the remoting infrastructure provides the
        /// necessary support for passing the arguments to the actual object across the remoting
        /// boundaries, calling the actual object method with the arguments, and returning the
        /// results back to the client of the proxy object.
        /// </summary>
        /// <param name="nextSink">Sets the next message sink in the sink chain.</param>
        public AspectTrace(IMessageSink nextSink)
        {
            this.NextSink = nextSink;
        }

        /// <summary>
        /// Asynchronously processes the given message. (I am not Implement the method)
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <param name="replySink">The reply sink for the reply message.</param>
        /// <returns>
        /// Returns an IMessageCtrl interface that provides a way to control asynchronous messages
        /// after they have been dispatched.
        /// </returns>
        public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
        {
            IMessageCtrl oMessage = null;

            IMethodCallMessage call = msg as IMethodCallMessage;

            var oAttribute = (LogAspectAttribute)Attribute.GetCustomAttribute(call.MethodBase, typeof(LogAspectAttribute));

            if (oAttribute == null)
            {
                oMessage = this.NextSink.AsyncProcessMessage(msg, replySink);
            }
            else
            {
                oAttribute.OnMethodExecuting(call);

                oMessage = this.NextSink.AsyncProcessMessage(msg, replySink);

                oAttribute.OnMethodExecuted(oMessage as IMethodReturnMessage);
            }

            return oMessage;
        }

        /// <summary>
        /// Synchronously processes the given message.
        /// </summary>
        /// <param name="msg">The message to process.</param>
        /// <returns>A reply message in response to the request.</returns>
        public IMessage SyncProcessMessage(IMessage msg)
        {
            IMessage oMessage = null;

            IMethodCallMessage call = msg as IMethodCallMessage;

            var oAttribute = (LogAspectAttribute)Attribute.GetCustomAttribute(call.MethodBase, typeof(LogAspectAttribute));

            if (oAttribute == null)
            {
                oMessage = this.NextSink.SyncProcessMessage(msg);
            }
            else
            {
                oAttribute.OnMethodExecuting(call);

                oMessage = this.NextSink.SyncProcessMessage(msg);

                oAttribute.OnMethodExecuted(oMessage as IMethodReturnMessage);
            }

            return oMessage;
        }
    }
}
