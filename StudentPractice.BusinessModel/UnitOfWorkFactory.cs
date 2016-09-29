using Imis.Domain;

namespace StudentPractice.BusinessModel
{
    public class UnitOfWorkFactory
    {
        public static IUnitOfWork Create()
        {
            var ctx = new DBEntities();
            ctx.MetadataWorkspace.LoadFromAssembly(ctx.GetType().Assembly);

            return ctx;
        }
    }
}
