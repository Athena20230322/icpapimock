using ICP.Modules.Mvc.Payment.Repositories.Event;

namespace ICP.Modules.Mvc.Payment.Services.Event
{
    public class ActivityService
    {
        private readonly ActivityRepository _activityRepository  = null;

        public ActivityService(ActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }
    }
}
