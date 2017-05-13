using System.Collections.Generic;
using System.Threading.Tasks;

namespace Piller.Services
{
    public interface INotificationService
    {
        Task<List<int>> ScheduleNotification(Data.MedicationDosage medication);
        void CancelNotification(int id);
        void CancelNotifications(IEnumerable<int> ids);
    }
}
