using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ICP.Infrastructure.Core.Helpers
{
    public class PropertyHelper<T>
    {
        public PropertyHelper()
        {

        }

        public PropertyHelper(T model)
        {

        }

        private MemberExpression GetMemberExpr(Expression<Func<T, dynamic>> expr)
        {
            MemberExpression me;
            switch (expr.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expr.Body as UnaryExpression;
                    me = ((ue != null) ? ue.Operand : null) as MemberExpression;
                    break;
                default:
                    me = expr.Body as MemberExpression;
                    break;
            }

            return me;
        }

        public string GetPropName(Expression<Func<T, dynamic>> expr)
        {
            var me = GetMemberExpr(expr);

            var propertyNames = new List<string>();

            while (me != null)
            {
                propertyNames.Add(me.Member.Name);

                me = me.Expression as MemberExpression;
            }

            propertyNames.Reverse();

            return string.Join(".", propertyNames);
        }

        public List<string> GetPropNames(params Expression<Func<T, dynamic>>[] exprs)
        {
            var result = new List<string>();

            foreach (var expr in exprs)
            {
                result.Add(GetPropName(expr));
            }

            return result;
        }

        public string GetMemberName(Expression<Func<T, dynamic>> expr)
        {
            var me = GetMemberExpr(expr);

            return me.Member.Name;
        }

        public List<string> GetMemberNames(params Expression<Func<T, dynamic>>[] exprs)
        {
            var result = new List<string>();

            foreach (var expr in exprs)
            {
                result.Add(GetMemberName(expr));
            }

            return result;
        }
    }
}