namespace jornal.Services;

public class CheckNull
{
    public bool IsNull(string value)
    {
        if (value == null)
            return true;

        return string.IsNullOrEmpty(value);
    }
}
