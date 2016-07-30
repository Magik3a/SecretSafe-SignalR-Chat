namespace SecretSafe.Infrastructure.Mapping
{
    using AutoMapper;

    public interface ICustomMapping
    {
        void CreateMappings(IConfiguration config);
    }
}