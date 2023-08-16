using System.Text.Json;
using System.Text;

namespace Sample.WebAPI.Policy
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            var snakeCaseName = new StringBuilder();
            bool prevCharWasUpper = false;

            for (int i = 0; i < name.Length; i++)
            {
                char currentChar = name[i];
                if (char.IsUpper(currentChar))
                {
                    if (prevCharWasUpper && i > 0)
                    {
                        snakeCaseName.Append(currentChar);
                    }
                    else
                    {
                        snakeCaseName.Append('_');
                        snakeCaseName.Append(char.ToLower(currentChar));
                    }

                    prevCharWasUpper = true;
                }
                else
                {
                    snakeCaseName.Append(currentChar);
                    prevCharWasUpper = false;
                }
            }

            return snakeCaseName.ToString();
        }
    }
}
