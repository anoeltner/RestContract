// <auto-generated />
[assembly: Codeworx.Rest.RestProxy(typeof(global::Codeworx.Rest.UnitTests.Api.Contract.IAuthorizedAction), typeof(Codeworx.Rest.UnitTests.Generated.AuthorizedActionClient))]
namespace Codeworx.Rest.UnitTests.Generated
{
    public class AuthorizedActionClient : Codeworx.Rest.Client.RestClient<global::Codeworx.Rest.UnitTests.Api.Contract.IAuthorizedAction>, global::Codeworx.Rest.UnitTests.Api.Contract.IAuthorizedAction
    {
        public AuthorizedActionClient(Codeworx.Rest.Client.RestOptions<global::Codeworx.Rest.UnitTests.Api.Contract.IAuthorizedAction> options) : base(options)
        {
        }

        public global::System.Threading.Tasks.Task AnonymousAsync()
        {
            return CallAsync(c => c.AnonymousAsync());
        }

        public global::System.Threading.Tasks.Task DenyAnonymousAsync()
        {
            return CallAsync(c => c.DenyAnonymousAsync());
        }
    }
}