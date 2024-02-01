using MethodaTest.Model;

namespace MethodaTest.Database;

public class AppRepository // didnt implament in memo db cuz of time pressure
{
    public List<Status> Statuses { get; set; } = new List<Status>();
    public int StatusId = 1;

    public List<Transition> Transitions { get; set; } = new List<Transition>();
    public int TransitionId = 1;

    public void AddStatus(Status newStatus)
    {
        Statuses.Add(newStatus);
    }

    public void UpdateStatus(Status updatedStatus)
    {
        var existingStatus = Statuses.FirstOrDefault(s => s.Id == updatedStatus.Id);

        if (existingStatus != null)
        {
            existingStatus.Name = updatedStatus.Name;
            existingStatus.IsInitial = updatedStatus.IsInitial;
            existingStatus.IsFinal = updatedStatus.IsFinal;
        }
    }

    public void DeleteStatus(int statusId)
    {
        var statusToRemove = Statuses.FirstOrDefault(s => s.Id == statusId);

        if (statusToRemove != null)
        {
            Transitions.RemoveAll(t => t.FromStatusId == statusToRemove.Id || t.ToStatusId == statusToRemove.Id);
            Statuses.Remove(statusToRemove);
        }
    }

    public void AddTransition(Transition newTransition)
    {
        Transitions.Add(newTransition);
    }

    public void DeleteTransition(int transitionId)
    {
        var transitionToRemove = Transitions.FirstOrDefault(t => t.Id == transitionId);

        if (transitionToRemove != null)
        {
            Transitions.Remove(transitionToRemove);
        }
    }

    public void InferLabel()
    {
        if (Statuses.Count == 0) return;

        foreach (var status in Statuses) 
        {
            status.IsInitial = false;
            status.IsOrphan = true;
            status.IsFinal = true;
        }
        Statuses[0].IsInitial = true;
        Statuses[0].IsOrphan = false;

        SetFinalsAndOrphans();

        Statuses[0].IsOrphan = false;
    }

    private void SetFinalsAndOrphans()
    {
        var usedStatusIds = new HashSet<int>();

        foreach (var transition in Transitions)
        {
            Statuses[transition.FromStatusId - 1].IsFinal = false;
            usedStatusIds.Add(transition.ToStatusId);

            // If the FromStatusId is an orphan his follow up is also an orphan
            if (!Statuses[transition.FromStatusId - 1].IsOrphan)
                Statuses[transition.ToStatusId - 1].IsOrphan = false;
        }

        // Find the first orphan (a status not used as the ToStatusId in any transition)
        var firstOrphan = Statuses.FirstOrDefault(status => !usedStatusIds.Contains(status.Id));

        if (firstOrphan is not null)
            firstOrphan.IsOrphan = true;
    }

}
