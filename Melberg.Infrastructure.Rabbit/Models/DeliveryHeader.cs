using System.Text;

namespace Melberg.Infrastructure.Rabbit.Models
{
        public class DeliveryHeader
	{
		public ulong DeliveryTag { get; set; }

		public bool Redelivered { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append("{");
			AppendAsEscapedValue(sb, "Redelivered", Redelivered);
			AppendComma(sb);
			AppendAsEscapedValue(sb, "DeliveryTag", DeliveryTag);
            TrimLastComma(sb);
			sb.Append("}");

			return sb.ToString();
		}

		private static void AppendAsEscapedValue(StringBuilder sb, string key, bool value)
		{
            if (key == null || value == default(bool)) return;

            sb.Append($"\"{EscapeString(key)}\"");
            sb.Append(":");
            sb.Append(value.ToString());
        }

		private static void AppendAsEscapedValue(StringBuilder sb, string key, ulong value)
		{
            if (key == null || value == default(ulong)) return;

            sb.Append($"\"{EscapeString(key)}\"");
            sb.Append(":");
            sb.Append(value.ToString());
        }

		private static void AppendComma(StringBuilder sb)
		{
            if (sb == null || sb.Length <= 1) return;

            var lastChar = sb[sb.Length - 1];
            switch (lastChar)
            {
                case '[':
                case '{':
                case ',':
                    break;
                default:
                    sb.Append(",");
                    break;
            }
        }

        private static void TrimLastComma(StringBuilder sb)
        {
            if (sb == null || sb.Length <= 0) return;

            var lastChar = sb[sb.Length - 1];
            if (lastChar == ',')
            {
                sb.Remove(sb.Length - 1, 1);
            }
        }

		private static string EscapeString(string value)
		{
            return value?.Replace("\"", "\\\"");
        }
    }
}