using System.Threading.Tasks;

namespace ScrumAdministrator.CockpitUwp.Model
{
    public interface IDataService
    {
        Task<DataItem> GetData();
    }
}