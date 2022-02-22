namespace Styleline.WinAnalyzer.DAL.Entities
{
    using System;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public static class ObservableBaseEx
    {
        public static string GetPropertyName<T, TProperty>(this T owner, Expression<Func<T, TProperty>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                UnaryExpression unaryExpression = expression.Body as UnaryExpression;
                if (unaryExpression == null)
                {
                    throw new NotImplementedException();
                }
                memberExpression = unaryExpression.Operand as MemberExpression;
                if (memberExpression == null)
                {
                    throw new NotImplementedException();
                }
            }
            return memberExpression.Member.Name;
        }

        public static void RaisePropertyChanged<T, TProperty>(this T observableBase, Expression<Func<T, TProperty>> expression) where T: NotifyChangedObservable
        {
            observableBase.RaisePropertyChanged(observableBase.GetPropertyName<T, TProperty>(expression));
        }
    }
}

