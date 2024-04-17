using System.Text;
using EmailParser.Entities;

namespace EmailParser.Validators;

public class SpamValidator : IValidator<CustomerRequest>
{
    public bool Validate(CustomerRequest candidate)
    {
        return !string.IsNullOrWhiteSpace(candidate.Phone);
    }

    private bool IsPhone(string contact)
    {
        var clearPhone = ClearPhone(contact);

        return false;
    }

    private string? ClearPhone(string phone)
    {
        var builder = new StringBuilder();

        foreach (var digit in phone)
        {
            if (char.IsDigit(digit))
            {
                builder.Append(digit);
            }

            if (char.IsLetter(digit))
            {
                return null;
            }
        }

        return builder.ToString();
    }
}