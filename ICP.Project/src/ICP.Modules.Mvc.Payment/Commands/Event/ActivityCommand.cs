using ICP.Modules.Mvc.Payment.Services.Event;

namespace ICP.Modules.Mvc.Payment.Commands.Event
{
    public class ActivityCommand
    {
        private readonly ActivityService _activityService = null;

        public ActivityCommand(ActivityService activityService)
        {
            _activityService = activityService;
        }
    }
}
