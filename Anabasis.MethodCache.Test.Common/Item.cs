
using System.Text;

namespace Anabasis.MethodCache.Test.Common
{
    public class Item : IFormattable
    {
        private readonly int _value;

        public Item(int value)
        {
            _value = value;
        }

        public void Format(StringBuilder stringBuilder)
        {
            stringBuilder.Append(_value);
        }
    }
}
