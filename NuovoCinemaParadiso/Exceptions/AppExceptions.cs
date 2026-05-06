namespace NuovoCinemaParadiso.Exceptions;

public abstract class AppException : Exception
{
    protected AppException(string message) : base(message) { }
}

public class NotFoundException : AppException
{
    public NotFoundException(string risorsa, string id)
        : base($"{risorsa} con ID '{id}' non trovato.") { }
}

public class ItemAlredyexist : AppException
{
    public ItemAlredyexist(string risorsa)
        : base($"Una {risorsa} è già collegata all'utente") { }
}

public class ConflictException : AppException
{
    public ConflictException(string message) : base(message) { }
}

public class ModificaException : AppException
{
    public ModificaException(string message) : base($"E' gia presente un {message} con lo stesso nome") { }
}

public class ItemNotFoundException : AppException
{
    public ItemNotFoundException(string message) : base($"{message} non trovato.") { }
}