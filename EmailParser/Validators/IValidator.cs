namespace EmailParser.Validators;

public interface IValidator<T>
{
    bool Validate(T candidate);
}