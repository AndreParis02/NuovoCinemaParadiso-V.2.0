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

public class GiftCardAlredyexis : AppException
{
    public GiftCardAlredyexis(string risorsa, string id)
        : base($"La {risorsa} con ID '{id}' è già collegata all'utente") { }
}

public class AbbonamentoAlredyexist : AppException
{
    public AbbonamentoAlredyexist(string risorsa, string id)
        : base($"L'{risorsa} con ID '{id}' è già collegata all'utente") { }
}

public class ConflictException : AppException
{
    public ConflictException(string message) : base(message) { }
}